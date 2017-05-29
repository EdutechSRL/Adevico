Namespace lm.Comol.Modules.UserActions.Presentation
    <CLSCompliant(True)> Public Enum StatisticView
        MySystem = 0
        UsersSystem = 1
        System = 2
        UserSystem = 3
        UserCommunitySystem = 4
        MyCommunity = 5
        UsersCommunity = 6
        Community = 7
        UserCommunity = 8
    End Enum


    <CLSCompliant(True)> Public Enum DetailViewType
        None = -100
        Day = 0
        Week = 1
        Month = 2
        Year = 3
        Range = 4
    End Enum

    <CLSCompliant(True)> Public Enum GroupingBy
        None = 0
        CommunityID = 1
        ModuleID = 2
        PersonID = 3
    End Enum
End Namespace