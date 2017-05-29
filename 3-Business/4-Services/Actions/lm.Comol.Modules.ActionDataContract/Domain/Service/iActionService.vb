Imports System.Runtime.Serialization
Imports System.ServiceModel

Namespace lm.ActionDataContract
    <ServiceContract()> _
    Public Interface iActionService

        <OperationContract(IsOneWay:=True)> _
        Sub InsertLoginAction(ByVal oLoginAction As LoginAction)

        <OperationContract(IsOneWay:=True)> _
        Sub UpdateLoginAction(ByVal oLoginAction As LoginAction)

        <OperationContract(IsOneWay:=True)> _
        Sub InsertUserAction(ByVal oAction As UserAction)

        <OperationContract(IsOneWay:=True)> _
       Sub InsertBrowserInfo(ByVal oBrowser As BrowserInfo)

        <OperationContract(IsOneWay:=True)> _
       Sub UpdateModuleUsageTime(ByVal oUsage As ModuleUsageTime)

        <OperationContract(IsOneWay:=True)> _
       Sub InsertCommunityAction(ByVal oAction As CommunityAction)

        <OperationContract(IsOneWay:=True)> _
       Sub UpdateCommunityAction(ByVal oAction As CommunityAction)

        <OperationContract(IsOneWay:=True)> _
        Sub InsertModuleAction(ByVal oAction As ModuleAction)

        <OperationContract(IsOneWay:=True)> _
       Sub UpdateModuleAction(ByVal oAction As ModuleAction)

    End Interface
End Namespace