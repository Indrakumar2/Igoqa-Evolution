import BudgetMonetary from './budgetMonetary';
import  { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import {
    FetchBudget,
} from '../../../viewComponents/dashboard/dahboardActions';
import { isEmptyReturnDefault } from '../../../../utils/commonUtils';
const mapStateToProps = (state) => {
  return {
    budgetMonetary:isEmptyReturnDefault(state.dashboardReducer.budget)
  };
};
const mapDispatchToProps = dispatch => {
    return {
      
      actions: bindActionCreators(
        {           
            FetchBudget,
         
        }, 
        dispatch
      ),
    };
  };

export default connect(mapStateToProps,mapDispatchToProps)(BudgetMonetary);