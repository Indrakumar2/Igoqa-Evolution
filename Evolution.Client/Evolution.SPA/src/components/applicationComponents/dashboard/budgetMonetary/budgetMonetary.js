import React,{ Component } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData }   from './headerData.js';
import PropTypes from 'proptypes';
import { generateBudgetArray } from '../budgetUtils/budgetUtils';
import { getlocalizeData } from '../../../../utils/commonUtils';
const localConstant = getlocalizeData();

class BudgetMonetary extends Component{

  constructor()
  {
   
    super();
    this.budgetArray =[];
       this.groupingParam = 
        { 
          groupName:"groupName",
          dataName:"childArray" 
        };
  }

  componentDidMount(){
    this.props.actions.FetchBudget('budgetMonetary');
  }

    render(){ 
  
        const rowData =generateBudgetArray(this.props.budgetMonetary);
        const headData = HeaderData;  
        return(
          <div>
             <ReactGrid gridRowData={rowData} gridColData={headData}
              isGrouping={true} 
              groupName={this.groupingParam && this.groupingParam.groupName} 
              dataName={this.groupingParam && this.groupingParam.dataName}    
              rowClassRules={{ allowDangerTag: true }} 
              rowName="BudgetMonetary"
              paginationPrefixId={localConstant.paginationPrefixIds.dashboardBudgetGridId}
            />
          </div>
          
        );
    }
}
BudgetMonetary.prototypes = {
    headData:PropTypes.array,
    rowData:PropTypes.array
};
BudgetMonetary.defaultprops ={
    headData:[],
    rowData:null
};
export default BudgetMonetary;