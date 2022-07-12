import AuditSearch from './auditSearch';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import {
    FetchModuleName,
    FetchSubModuleName,
    FetchAuditSearchData,
    ClearMouduleName,
    ClearParentData,
    FetchAuditEventData,
    FetchAuditLogData
} from '../../../actions/auditSearch/auditSearchAction';
import { isEmptyReturnDefault } from '../../../utils/commonUtils';
const mapStateToProps = (state) => {
    return {
        moduleData: isEmptyReturnDefault(state.auditSearchReducer.moduleData),
        submoduleData: isEmptyReturnDefault(state.auditSearchReducer.submoduleData),
        auditSearchUnquieId:isEmptyReturnDefault(state.auditSearchReducer.auditSearchUnquieId),
        auditSearchData:isEmptyReturnDefault(state.auditSearchReducer.auditSearchData),
        auditSubGridData:isEmptyReturnDefault(state.auditSearchReducer.auditSubGridData),
    };
};
const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchModuleName,
                FetchSubModuleName,
                FetchAuditSearchData,
                ClearMouduleName,
                ClearParentData,
                FetchAuditEventData,
                FetchAuditLogData

            },
            dispatch
        ),
    };
};
export default (connect(mapStateToProps, mapDispatchToProps)(AuditSearch));
