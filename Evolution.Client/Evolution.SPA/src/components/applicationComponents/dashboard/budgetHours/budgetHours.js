import React,{ Component } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './headerData.js';
import PropTypes from 'proptypes';
import { generateBudgetArray } from '../budgetUtils/budgetUtils';
import { getlocalizeData } from '../../../../utils/commonUtils';
const localConstant = getlocalizeData();

class BudgetHours extends Component{
    constructor()
    {
     
      super();
         this.groupingParam = 
          { 
            groupName:"groupName",
            dataName:"childArray" 
          };
    }
    componentDidMount(){
        this.props.actions.FetchBudget('budgetHour');
    }

    render(){  
        
        const rowData =generateBudgetArray(this.props.budgetHours);
        const headData = HeaderData;
        return(
            <ReactGrid gridRowData={rowData} gridColData={headData}
            isGrouping={true} 
            groupName={this.groupingParam && this.groupingParam.groupName} 
            dataName={this.groupingParam && this.groupingParam.dataName} 
            rowClassRules={{ allowDangerTag: true }} 
            rowName="BudgetHours"
            paginationPrefixId={localConstant.paginationPrefixIds.dashboardBudgetHourGridId}
                />
        );
    }
}
BudgetHours.prototypes = {
    headData:PropTypes.array.isrequired,
    rowData:PropTypes.array.isrequired
};
BudgetHours.defaultprops ={
    headData:[],
    rowData:[]
};
export default BudgetHours;