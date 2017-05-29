using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lm.Comol.Modules.CallForPapers.Domain;

namespace lm.Comol.Modules.CallForPapers.Presentation
{
    public interface IViewViewSubmission : IViewViewBaseSubmission
    {
        Boolean PreloadAscending { get; }
        SubmissionFilterStatus PreloadFilterSubmission { get; }
        SubmissionsOrder PreloadOrderSubmission { get; }
        Boolean PreloadFromManagement { get; }
        int PreloadPageIndex { get; }
        int PreloadPageSize { get; }

        Boolean ShowAdministrationTools { get; set; }
        Boolean AllowRevisionRequest { get; set; }


        void LoadAvailableStatus(List<SubmissionStatus> items);
        void UpdateStatus(SubmissionStatus status);

        long IdPendingRevision { get; set; }
        void LoadAvailableRevision(List<dtoRevision> revisions, long idSelected);
        void InitializeRevisionRequest(RevisionType type);
        void DisplayRevisionInfo(dtoRevision revision);
        void DisplaySelfPendingRequest(dtoRevisionRequest revision, String url);
        void DisplayUserPendingRequest(dtoRevisionRequest revision, String url);
        void DisplayPendingRevision(dtoRevisionRequest revision, String url);
        void DisplayManagePendingRevision(dtoRevisionRequest revision, String url);
        List<dtoRevisionItem> GetFieldsToReview();
    }
}
