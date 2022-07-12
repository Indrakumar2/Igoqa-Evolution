import HistorialVisit from './historialVisit';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { isEmptyReturnDefault, } from '../../../../utils/commonUtils';
import { FetchHistoricalVisit } from '../../../../actions/visit/historicalVisitAction';

const mapStateToProps = (state) => { 
    return {        
        historicalVisits: isEmptyReturnDefault(state.rootVisitReducer.historicalVisits),
        currentPage: state.CommonReducer.currentPage,
        visitInfo: isEmptyReturnDefault(state.rootVisitReducer.visitDetails.VisitInfo, 'object')
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {
                FetchHistoricalVisit
            },
            dispatch
        ),
    };
};
export default connect(mapStateToProps, mapDispatchToProps)(withRouter(HistorialVisit));