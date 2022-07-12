import Documents from './documents';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import {
    FetchTimesheetDocuments,
    AddTimesheetDocumentDetails,
    UpdateTimesheetDocumentDetails,
    DeleteTimesheetDocumentDetails,
    AddFilesToBeUpload
} from '../../../../actions/timesheet/timesheetDocumentAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import {
    FetchDocumentUniqueName,
    UploadDocumentData,
    PasteDocumentUploadData,
    MultiDocDownload,
} from '../../../../common/commonAction';
import { DisplayModal, HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { filterDocTypes } from '../../../../common/selector';
import { isApprovedByCoordinator,isOperatorCompany,isCoordinatorCompany } from '../../../../selectors/timesheetSelector';
import { isInterCompanyAssignment } from '../../../../selectors/assignmentSelector';

const mapStateToProps = (state) => { 
    const timesheetInfo = isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetInfo, 'object');   
    return {
        timesheetDocuments:isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.TimesheetDocuments),
        timesheetId: timesheetInfo?timesheetInfo.timesheetId:0,
        timesheetDocTypes:filterDocTypes({ docTypes:state.masterDataReducer.documentTypeMasterData, moduleName:'Visit' }),
        pageMode:state.CommonReducer.currentPageMode,
        timesheetInfo:timesheetInfo,
        isApproved_OC_CHC : isApprovedByCoordinator( timesheetInfo ),
        isOperatorCompany:isOperatorCompany(timesheetInfo.timesheetOperatingCompanyCode,state.appLayoutReducer.selectedCompany),
        isCoordinatorCompany:isCoordinatorCompany(timesheetInfo.timesheetContractCompanyCode,state.appLayoutReducer.selectedCompany),
        iInterCompanyAssignment: isInterCompanyAssignment(timesheetInfo.timesheetOperatingCompanyCode, timesheetInfo.timesheetContractCompanyCode),
        fileToBeUploaded:isEmptyReturnDefault(state.rootTimesheetReducer.timesheetDetail.fileToBeUploaded),
        };
    };

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchTimesheetDocuments,
                AddTimesheetDocumentDetails,
                UpdateTimesheetDocumentDetails,
                DeleteTimesheetDocumentDetails,
                FetchDocumentUniqueName,
                UploadDocumentData,
                PasteDocumentUploadData,
                DisplayModal, HideModal,
                MultiDocDownload,
                AddFilesToBeUpload
            },
            dispatch
        ),
    };
};
export default withRouter(connect(mapStateToProps, mapDispatchToProps)(Documents));