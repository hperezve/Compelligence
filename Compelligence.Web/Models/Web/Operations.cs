using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Compelligence.Web.Models.Web
{
    public enum ActionMethod
    {
        Unknown,
        Create,
        Edit,
        EditPassword,
        Delete,
        Duplicate
    }

    public enum SecurityButtonAction
    {
        Save,
        Reset,
        Cancel,
        NewDetail,
        AddDetail,
        EditDetail,
        DeleteDetail,
        DuplicateDetail,
        Sort,
        Execute,
        GetData,
        ForceRead,
        CancelDetail
    }

    public enum SecurityButtonType
    {
        Submit,
        Button,
        Reset,
        Image
    }


    public enum OperationStatus
    {
        Initiated = 100,
        Successful = 200,
        Unsuccessful = 400
    }

    public enum DetailType
    {
        Budget = 1000,
        Answer,
        Competitor,
        Customer,
        Discussion,
        Employee,
        Feedback,
        File,
        Implication,
        Industry,
        Kit,
        Library,
        Label,
        Location,
        Metric,
        Objective,
        Plan,
        Positioning,
        Product,
        Project,
        ProductCriteria,
        Question,
        Related,
        Respond,
        Results,
        Team,
        User,
        TeamRole,
        TeamMember,
        CompetitorCriteria,
        IndustryCriteria,
        Comment,
        CriteriaGroup,
        ApprovalList,
        Source,
        EntityRelation,
        QuizClassification,
        Event,
        Closed, //For Deal Close
        CompetitorPartner,
        Criteria,
        LibraryNews,
        ProductFamily,
        CompetitorSupplier,
        CompetitorCompetitor,
        ActionHistory,
        IndustryFinancial,
        UserProfile,
        MarketType,
        Trend,
        UserRelation,
        Threat,
        CompetitorFinancial,
        Price,
        News,
        CompetitiveMessaging,
        StrengthWeakness,
        HistoryField
    }

    public enum DetailCreateType
    {
        Clone,
        Override
    }
}