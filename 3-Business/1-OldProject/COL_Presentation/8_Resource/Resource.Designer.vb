﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:2.0.50727.3053
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Friend Class Resource
        
        Private Shared resourceMan As Global.System.Resources.ResourceManager
        
        Private Shared resourceCulture As Global.System.Globalization.CultureInfo
        
        <Global.System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>  _
        Friend Sub New()
            MyBase.New
        End Sub
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Shared ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("PresentationLayer.Resource", GetType(Resource).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Shared Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to All.
        '''</summary>
        Friend Shared ReadOnly Property ALLorganization() As String
            Get
                Return ResourceManager.GetString("ALLorganization", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Community.
        '''</summary>
        Friend Shared ReadOnly Property CommunityFolder() As String
            Get
                Return ResourceManager.GetString("CommunityFolder", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to All.
        '''</summary>
        Friend Shared ReadOnly Property CommunityStatus_All() As String
            Get
                Return ResourceManager.GetString("CommunityStatus.All", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Archiviata.
        '''</summary>
        Friend Shared ReadOnly Property CommunityStatus_Archiviata() As String
            Get
                Return ResourceManager.GetString("CommunityStatus.Archiviata", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Closed By Administrator.
        '''</summary>
        Friend Shared ReadOnly Property CommunityStatus_ClosedByAdministration() As String
            Get
                Return ResourceManager.GetString("CommunityStatus.ClosedByAdministration", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Not found..
        '''</summary>
        Friend Shared ReadOnly Property CommunityStatus_None() As String
            Get
                Return ResourceManager.GetString("CommunityStatus.None", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Only Activated.
        '''</summary>
        Friend Shared ReadOnly Property CommunityStatus_OnlyActivated() As String
            Get
                Return ResourceManager.GetString("CommunityStatus.OnlyActivated", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to No event is been imported into community &quot;{0}&quot;..
        '''</summary>
        Friend Shared ReadOnly Property Esse3ImportToComol_Error() As String
            Get
                Return ResourceManager.GetString("Esse3ImportToComol_Error", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to All events are been imported into community &quot;{0}&quot;..
        '''</summary>
        Friend Shared ReadOnly Property Esse3ImportToComol_OK() As String
            Get
                Return ResourceManager.GetString("Esse3ImportToComol_OK", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Some events are not be imported into community &quot;{0}&quot;, in particular:&lt;br&gt;{1}.
        '''</summary>
        Friend Shared ReadOnly Property Esse3ImportToComol_PartialOk() As String
            Get
                Return ResourceManager.GetString("Esse3ImportToComol_PartialOk", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to User Folders.
        '''</summary>
        Friend Shared ReadOnly Property UserFolder() As String
            Get
                Return ResourceManager.GetString("UserFolder", resourceCulture)
            End Get
        End Property
    End Class
End Namespace