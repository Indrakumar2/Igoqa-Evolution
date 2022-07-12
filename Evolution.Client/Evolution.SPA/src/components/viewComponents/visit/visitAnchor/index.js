import VisitAnchor from './visitAnchor';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { GetSelectedVisit } from '../../../../actions/visit/visitAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { SetCurrentPageMode } from '.././../../../common/commonAction';
const mapStateToProps = (state) => {    
    return{
        selectedCompany: state.appLayoutReducer.selectedCompany,
        selectedCompanyData: isEmptyReturnDefault(state.appLayoutReducer.selectedCompanyData,'object'),
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {  
                GetSelectedVisit,
                SetCurrentPageMode
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(VisitAnchor);