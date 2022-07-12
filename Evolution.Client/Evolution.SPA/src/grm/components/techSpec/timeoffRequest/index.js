import TimeOffRequest from './timeoffRequest';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { FetchTimeOffRequestData,FetchCategory,TimeOffRequestSave,getEmploymentType } from '../../../actions/techSpec/timeOffRequestAction';
import { TechSpechUnSavedData } from '../../../actions/techSpec/preAssignmentAction';
import {
    FetchCalendarData
  } from '../../../actions/techSpec/globalCalendarAction';

const mapStateToProps = (state) => {
    return {
        resourceNameList: state.RootTechSpecReducer.TechSpecDetailReducer.resourceNameList,
        employmentTypeList:state.masterDataReducer.employmentStatus,
        category: state.RootTechSpecReducer.TechSpecDetailReducer.timeOffRequestCategory,
        timeoffRequestDetails:state.RootTechSpecReducer.TechSpecDetailReducer.timeoffRequestDetails,
        resourceDetails:state.RootTechSpecReducer.TechSpecDetailReducer.resourceDetails,
        selectedCompany: state.appLayoutReducer.selectedCompany,    
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {              
                FetchTimeOffRequestData,
                FetchCategory,
                TimeOffRequestSave,
                getEmploymentType,
                TechSpechUnSavedData,
                FetchCalendarData
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(withRouter(TimeOffRequest));