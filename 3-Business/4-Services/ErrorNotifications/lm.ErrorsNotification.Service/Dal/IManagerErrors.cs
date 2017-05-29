using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.ErrorsNotification.DataContract.Domain;

namespace lm.ErrorsNotification.Service.Dal
{
    interface IManagerErrors
    {
          void SaveCommunityModuleError(CommunityModuleError error);
          void SaveDBerror(DBerror error);
          void SaveGenericError(GenericError error);
          void SaveGenericModuleError(GenericModuleError error);
          void SaveGenericWebError(GenericWebError error);
          void SaveFileError(FileError error);
    }
}
