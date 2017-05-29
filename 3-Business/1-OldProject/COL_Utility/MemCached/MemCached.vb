Imports Enyim.Caching

Public Class MemCached
    Private Shared _MemClient As Enyim.Caching.MemcachedClient = setup()

    Private Shared ReadOnly Property oMemCacheClient() As Enyim.Caching.MemcachedClient
        Get
            'If IsNothing(Me._MemClient) Then
            '    Me._MemClient = New Enyim.Caching.MemcachedClient()
            'End If
            Return _MemClient
        End Get
    End Property

    'Public Sub New()
    '    Me._MemClient = New Enyim.Caching.MemcachedClient
    'End Sub

    Public Shared Function setup() As Enyim.Caching.MemcachedClient
        Return New Enyim.Caching.MemcachedClient
    End Function

    Public Shared Property KeyHash() As Hashtable
        Get
            Dim hash As Hashtable
            'Dim oMemCacheClient As New Enyim.Caching.MemcachedClient

            hash = oMemCacheClient.Get(CachePolicy.CachedKey)

            If IsNothing(hash) Then
                hash = New Hashtable
            End If

            'If DistCache.KeyExists(CachePolicy.CachedKey) Then
            '    hash = DistCache.Get(CachePolicy.CachedKey)
            'Else
            '    hash = New Hashtable
            'End If

            Return hash
        End Get
        Set(ByVal value As Hashtable)
            If oMemCacheClient.Store(Enyim.Caching.Memcached.StoreMode.Set, CachePolicy.CachedKey, value) Then
            Else
                Dim str As String = ""
            End If

            'DistCache.Add(CachePolicy.CachedKey, value)
        End Set
    End Property
    Public Shared Sub AddKey(ByVal strKey As String, ByVal obj As Object, Optional ByVal spanMS As Long = 0)
        Dim hash As Hashtable = KeyHash

        Dim Scadenza As TimeSpan = CacheTime.Scadenza2minuti
        If Not spanMS = 0 Then
            Scadenza = New TimeSpan(spanMS * 10)
        End If

        If Not hash.ContainsKey(strKey) Then
            hash.Add(strKey, strKey)
            If Not oMemCacheClient.Store(Enyim.Caching.Memcached.StoreMode.Set, CachePolicy.CachedKey, hash, CacheTime.Scadenza2minuti) Then
                Dim str As String = ""
            End If
        End If

        'Try
        '    hash = oMemCacheClient.Get(CachePolicy.CachedKey)
        'Catch ex As Exception
        '    hash = New Hashtable
        'End Try

        'If IsNothing(hash) Then
        '    hash = New Hashtable
        'End If


        If Not oMemCacheClient.Store(Enyim.Caching.Memcached.StoreMode.Set, strKey, obj, CacheTime.Scadenza2minuti) Then
            Dim str As String = ""
        End If

        'If spanMS = 0 Then
        '    DistCache.Add(strKey, obj)
        'Else
        '    DistCache.Add(strKey, obj, spanMS)
        'End If

    End Sub
    Public Shared Sub AddKey(ByVal strKey As String, ByVal obj As Object, ByVal span As TimeSpan)
        AddKey(strKey, obj, span.TotalMilliseconds)
    End Sub
    Public Shared Function GetKey(ByVal strKey As String) As Object
        Return oMemCacheClient.Get(strKey)
    End Function
    Public Shared Function GetKeyLazy(ByVal strkey As String) As Object
        'Dim oMemCacheClient As New Enyim.Caching.MemcachedClient
        'Return DistCache.Get(strkey)
        Return oMemCacheClient.Get(strkey)
    End Function
    Public Shared Sub Remove(ByVal strkey As String)
        'Dim oMemCacheClient As New Enyim.Caching.MemcachedClient
        'Dim hash As Hashtable = KeyHash
        KeyHash.Remove(strkey)
        oMemCacheClient.Remove(strkey)
    End Sub

    ''' <summary>
    ''' Svuota TUTTA la cache: USARE CON PARSIMONIA!!!
    ''' </summary>
    ''' <remarks>Aggiungere lo SharedLogger</remarks>
    Public Shared Sub RemoveAll()
        'SharedLogger.Logger.Info("Cache svuotata")
        KeyHash = New Hashtable
        oMemCacheClient.FlushAll()
    End Sub

    ''' <summary>
    ''' Per cancellare le chiavi che iniziano con name. E' per questo che serve la hash table.
    ''' </summary>
    ''' <param name="name"></param>
    ''' <remarks></remarks>
    Public Shared Sub RemoveNamespace(ByVal name As String)
        Dim hash As Hashtable = KeyHash
        For Each key As String In hash.Keys
            If key.StartsWith(name) Then
                Remove(key)
            End If
        Next
    End Sub
    Public Shared Sub ReNew(ByVal strkey As String, Optional ByVal ms As Long = 0)
        'Dim oMemCacheClient As New Enyim.Caching.MemcachedClient
        Dim Scadenza As New TimeSpan(TimeSpan.TicksPerMillisecond * ms)
        Dim obj As Object
        If IsNothing(oMemCacheClient.Get(strkey)) Then
            'If DistCache.KeyExists(strkey) Then
            obj = GetKey(strkey)
            If ms = 0 Then
                'DistCache.Add(strkey, obj)
                oMemCacheClient.Store(Enyim.Caching.Memcached.StoreMode.Set, strkey, obj, MemcachedClient.Infinite)
            Else
                oMemCacheClient.Store(Enyim.Caching.Memcached.StoreMode.Set, strkey, obj, Scadenza)
                'DistCache.Add(strkey, obj, ms)
            End If
        End If
    End Sub
    Public Shared Sub ReNew(ByVal strkey As String, ByVal span As TimeSpan)
        ReNew(strkey, span)
    End Sub
    Public Shared Sub ReNewHashKey()
        ReNew(CachePolicy.CachedKey)
    End Sub
    Public Shared Sub ReNewHashKey(ByVal span As TimeSpan)
        ReNew(CachePolicy.CachedKey, span)
    End Sub
    Public Shared Function KeyExists(ByVal strkey As String) As Boolean
        'Dim oMemCacheClient As New Enyim.Caching.MemcachedClient
        Dim obj As Object = oMemCacheClient.Get(strkey)
        Return Not IsNothing(obj)
    End Function

    Public Shared Function GetStatStr(ByVal Ip As String, ByVal Port As Integer) As String
        Dim IpAddress As System.Net.IPEndPoint
        IpAddress = New System.Net.IPEndPoint(System.Net.IPAddress.Parse(Ip), Port)

        Dim str As String
        str = ""
        str &= "pid: " & oMemCacheClient.Stats.GetRaw(IpAddress, "pid") & "<br />"
        str &= "uptime: " & oMemCacheClient.Stats.GetRaw(IpAddress, "uptime") & "<br />"
        str &= "time: " & oMemCacheClient.Stats.GetRaw(IpAddress, "time") & "<br />"
        str &= "version: " & oMemCacheClient.Stats.GetRaw(IpAddress, "version") & "<br />"
        str &= "pointer_size: " & oMemCacheClient.Stats.GetRaw(IpAddress, "pointer_size") & "<br />"
        str &= "rusage_user: " & oMemCacheClient.Stats.GetRaw(IpAddress, "rusage_user") & "<br />"
        str &= "rusage_system: " & oMemCacheClient.Stats.GetRaw(IpAddress, "rusage_system") & "<br />"
        str &= "curr_items: " & oMemCacheClient.Stats.GetRaw(IpAddress, "curr_items") & "<br />"
        str &= "total_items: " & oMemCacheClient.Stats.GetRaw(IpAddress, "total_items") & "<br />"
        str &= "bytes: " & oMemCacheClient.Stats.GetRaw(IpAddress, "bytes") & "<br />"
        str &= "curr_connections: " & oMemCacheClient.Stats.GetRaw(IpAddress, "curr_connections") & "<br />"
        str &= "total_connections: " & oMemCacheClient.Stats.GetRaw(IpAddress, "total_connections") & "<br />"
        str &= "connection_structures: " & oMemCacheClient.Stats.GetRaw(IpAddress, "connection_structures") & "<br />"
        str &= "cmd_get: " & oMemCacheClient.Stats.GetRaw(IpAddress, "cmd_get") & "<br />"
        str &= "cmd_set: " & oMemCacheClient.Stats.GetRaw(IpAddress, "cmd_set") & "<br />"
        str &= "get_hits: " & oMemCacheClient.Stats.GetRaw(IpAddress, "get_hits") & "<br />"
        str &= "get_misses: " & oMemCacheClient.Stats.GetRaw(IpAddress, "get_misses") & "<br />"
        str &= "evictions: " & oMemCacheClient.Stats.GetRaw(IpAddress, "evictions") & "<br />"
        str &= "bytes_read: " & oMemCacheClient.Stats.GetRaw(IpAddress, "bytes_read") & "<br />"
        str &= "bytes_written: " & oMemCacheClient.Stats.GetRaw(IpAddress, "bytes_written") & "<br />"
        str &= "limit_maxbytes: " & oMemCacheClient.Stats.GetRaw(IpAddress, "limit_maxbytes") & "<br />"
        str &= "threads: " & oMemCacheClient.Stats.GetRaw(IpAddress, "threads") & "<br />"

        Return str
    End Function

    '#region [ readonly string[] StatKeys   ] 
    '   private static readonly string[] StatKeys =  
    '   { 
    '       "uptime", 
    '       "time", 
    '       "version", 
    '       "curr_items", 
    '       "total_items", 
    '       "curr_connections", 
    '       "total_connections", 
    '       "connection_structures", 
    '       "cmd_get", 
    '       "cmd_set", 
    '       "get_hits", 
    '       "get_misses", 
    '       "bytes", 
    '       "bytes_read", 
    '       "bytes_written", 
    '       "limit_maxbytes", 
    '   }; 
    '#endregion 
End Class
