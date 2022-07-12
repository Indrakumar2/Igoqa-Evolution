import AdditionalExpenses from './additionalExpenses';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { AddAdditionalExpenses,UpdateAdditionalExpenses,DeleteAdditionalExpenses } from '../../../../actions/assignment/assignmentAdditionalExpensesAction';
import { DisplayModal,HideModal } from '../../../../common/baseComponents/customModal/customModalAction';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
import { FetchExpenseType } from '../../../../common/masterData/masterDataActions';

const mapStateToProps = (state) => {    
    return{
        additionalExpenses: state.rootAssignmentReducer.assignmentDetail.AssignmentAdditionalExpenses,
        assignmentInfo: isEmptyReturnDefault(state.rootAssignmentReducer.assignmentDetail.AssignmentInfo, 'object'),
        currencyCodes: state.masterDataReducer.currencyMasterData,
        expenseTypes:state.masterDataReducer.expenseType,
        currentPage: state.CommonReducer.currentPage,
        interactionMode: state.CommonReducer.interactionMode,
        pageMode:state.CommonReducer.currentPageMode
    };
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            {  
                AddAdditionalExpenses,
                UpdateAdditionalExpenses,
                DeleteAdditionalExpenses,
                DisplayModal,
                HideModal,
                FetchExpenseType,
            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(AdditionalExpenses);