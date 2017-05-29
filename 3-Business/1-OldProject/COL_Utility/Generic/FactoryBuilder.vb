Imports System.Reflection

Public Class FactoryBuilder
    ''' <summary>
    '''    Create the requested Factory
    ''' </summary>
    ''' <typeparam name="t">type of</typeparam>
    ''' <param name="classname">class name</param>
    ''' <returns>new instance of t</returns>
    ''' <remarks></remarks>
    Public Shared Function BuildFactory(Of t)(ByVal classname As String) As t
        Dim obj As t
        obj = CreateObject(Of t)(classname)
        Return obj
    End Function

    ''' <summary>
    '''    Generic BuildFactory
    ''' </summary>
    ''' <param name="classname">class full name</param>
    ''' <returns>new instance of t</returns>
    ''' <remarks></remarks>
    Public Shared Function BuildFactory(ByVal classname As String) As Object
        Dim obj As Object
        Dim assem As Assembly = Assembly.GetExecutingAssembly
        obj = CreateObject(assem, classname)
        Return obj
    End Function

    ''' <summary>
    '''    Obtains a list of Type that implements the given interface
    ''' </summary>
    ''' <typeparam name="t">interface</typeparam>
    ''' <returns>list of type</returns>
    ''' <remarks></remarks>
    Public Shared Function iFactoryImplementations(Of t)() As IList
        Dim list As New ArrayList
        Dim assemb As Assembly = Assembly.GetAssembly(GetType(t))

        Dim types() As Type
        types = assemb.GetTypes()

        For Each eachType As Type In types

            If IsImplemented(eachType, GetType(t)) Then
                list.Add(eachType)
            End If
        Next

        Return list
    End Function

#Region "Create Object"

    ''' <summary>
    '''    Create an instance of a class, from assembly and class fullname
    ''' </summary>    
    ''' <param name="assemb">assembly</param>
    ''' <param name="fullname">class fullname</param>
    ''' <returns>a new object of requested class</returns>
    ''' <remarks></remarks>
    Private Shared Function CreateObject(ByVal assemb As Assembly, ByVal fullname As String) As Object
        Return assemb.CreateInstance(fullname)
    End Function

    ''' <summary>
    '''    Create an instance of a class, extracting the assembly from the type
    ''' </summary>
    ''' <typeparam name="t">type of</typeparam>
    ''' <param name="fullname">class fullname</param>
    ''' <returns>new instance of class requested</returns>
    ''' <remarks></remarks>
    Private Shared Function CreateObject(Of t)(ByVal fullname As String) As Object
        Dim assemb As Assembly = Assembly.GetAssembly(GetType(t))
        Return CreateObject(assemb, fullname)
    End Function

    ''' <summary>
    '''    Create an instance of a class, extracting the assembly from dll or assembly name
    ''' </summary>    
    ''' <param name="DLLorAssemblyname">path of dll, or assembly name</param>
    ''' <param name="fullname">class fullname</param>
    ''' <param name="isDLL">true: DLL , false: assembly name</param>
    ''' <returns>new instance of class requested</returns>
    ''' <remarks></remarks>

    Private Shared Function CreateObject(ByVal DLLorAssemblyname As String, ByVal fullname As String, Optional ByVal isDLL As Boolean = False) As Object
        Dim assemb As Assembly
        If isDLL Then
            assemb = Assembly.LoadFile(DLLorAssemblyname)
        Else
            assemb = Assembly.Load(DLLorAssemblyname)
        End If
        Return CreateObject(assemb, fullname)
    End Function
#End Region

#Region "General utilities"
    ''' <summary>
    '''    Check if a type implements an interface
    ''' </summary>
    ''' <param name="objectType">type to check</param>
    ''' <param name="interfaceType">interface that must be implemented</param>
    ''' <returns>check implementation</returns>
    ''' <remarks></remarks>
    Private Shared Function IsImplemented(ByVal objectType As Type, ByVal interfaceType As Type) As Boolean
        For Each thisInterface As Type In objectType.GetInterfaces
            If thisInterface Is interfaceType Then
                Return True
            End If
        Next
    End Function
#End Region

End Class