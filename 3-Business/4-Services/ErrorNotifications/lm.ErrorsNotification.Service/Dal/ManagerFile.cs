using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.ErrorsNotification.DataContract.Domain;

namespace lm.ErrorsNotification.Service.Dal
{
    public class ManagerFile: IManagerErrors
    {
        #region IManagerErrors Members

        public void SaveCommunityModuleError(CommunityModuleError error)
        {
            throw new NotImplementedException();
        }

        public void SaveDBerror(DBerror error)
        {
            throw new NotImplementedException();
        }

        public void SaveGenericError(GenericError error)
        {
            throw new NotImplementedException();
        }

        public void SaveGenericModuleError(GenericModuleError error)
        {
            throw new NotImplementedException();
        }

        public void SaveGenericWebError(GenericWebError error)
        {
            throw new NotImplementedException();
        }

        public void SaveFileError(FileError error)
        {
        }

        #endregion
    }
}
