Namespace lm.ActionDataContract
    <Serializable(), CLSCompliant(True)> Public Enum InteractionType 'Classe Attività
        None = 1
        ' Interaction between users
        UserWithUser = 2
        ' Interaction between user and community administrator
        UserWithCommunityAdministrator = 3
        ' Interaction between user and LearingObjects
        UserWithLearningObject = 4
        ' Interaction generic
        Generic = 5
        ' Interaction betweeen core
        SystemToSystem = 6
        ' Interaction from core to user
        SystemToUser = 7
        ' Interaction from core to module
        SystemToModule = 8
        ' Interaction from module to core
        ModuleToSystem = 9
        ' Interaction between modules
        ModuleToModule = 10
    End Enum
End Namespace