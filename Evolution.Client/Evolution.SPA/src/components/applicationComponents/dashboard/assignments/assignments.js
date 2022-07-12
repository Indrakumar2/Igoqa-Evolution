import React,{ Component,Fragment } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './activeAssignmentHeader.js';
import PropTypes from 'proptypes';
import { isEmpty,getlocalizeData } from '../../../../utils/commonUtils';

const localConstant = getlocalizeData();

class Assignments extends Component{
    componentDidMount() {
        this.props.actions.FetchActiveAssignments();   
    }
  
    render(){   
      
        const headData = HeaderData; 

        this.props.assignmentGridData && this.props.assignmentGridData.map(assignment => {    
                const technicalSpecialists = [];
                if(Array.isArray(assignment.techSpecialists) && assignment.techSpecialists.length>0){
                    assignment.techSpecialists.forEach((iterated)=>{  
                        if(iterated.fullName.trim()!=='')                      
                        technicalSpecialists.push(iterated.fullName);
                    });
                    assignment.techSpecialists = (technicalSpecialists).join(',');
                }
            });
            // Default Sort Order
            // Condition 1:
            //      Assignment techSpecialists || operating coordinator is null Then Sort By 1) ContractNumber, CustomerName, ProjectNumber, AssignmentNumber
            //
            //Condition 2:(Based on Evo1 N/A) - Because Assignment First Visit date will never be Null
            //      Assignment First Visit Date is null -> Sort should be maintained as Contract NUmber, ContractId, CustomerName, ProjectNumber, AssignmentNumber
            // Condition 3: 
            // When TS name is there and Operating coordinator name is there. Sort should be done by contract Number, Project Number , assignment Number

            // The same is applicable to Inactive Assignments

        const techSpecNullData = this.props.assignmentGridData.filter( item => isEmpty(item.techSpecialists) 
                                                                                || isEmpty(item.assignmentOperatingCompanyCoordinator) )
                                                               .sort(function(a, b){
                                                                   
                                                                    if(a.assignmentContractNumber > b.assignmentContractNumber)
                                                                        return 1;
                                                                    if(b.assignmentContractNumber > a.assignmentContractNumber)
                                                                        return -1;
                                                                        if(a.assignmentCustomerName > b.assignmentCustomerName)
                                                                        return 1;
                                                                    if(b.assignmentCustomerName > a.assignmentCustomerName)
                                                                        return -1;
                                                                    if(a.assignmentProjectNumber  > b.assignmentProjectNumber)
                                                                        return 1;
                                                                    if(b.assignmentProjectNumber > a.assignmentProjectNumber)
                                                                        return -1;
                                                                    if(a.assignmentNumber > b.assignmentNumber)
                                                                        return 1;
                                                                    if(b.assignmentNumber > a.assignmentNumber)
                                                                        return -1;
                                                               });
        const dataWithTechSpecData = this.props.assignmentGridData.filter( item => !isEmpty(item.techSpecialists) 
                                                                                && !isEmpty(item.assignmentOperatingCompanyCoordinator) )
                                                                    .sort(function(a,b)
                                                                    {
                                                                        // if(a.assignmentCustomerName > b.assignmentCustomerName)
                                                                        // return 1;
                                                                        // if(b.assignmentCustomerName > a.assignmentCustomerName)
                                                                        // return -1;
                                                                        if(a.assignmentContractNumber > b.assignmentContractNumber)
                                                                        return 1;
                                                                        if(b.assignmentContractNumber > a.assignmentContractNumber)
                                                                            return -1;
                                                                        if(a.assignmentProjectNumber  > b.assignmentProjectNumber)
                                                                            return 1;
                                                                        if(b.assignmentProjectNumber > a.assignmentProjectNumber)
                                                                            return -1;
                                                                        if(a.assignmentNumber > b.assignmentNumber)
                                                                            return 1;
                                                                        if(b.assignmentNumber > a.assignmentNumber)
                                                                            return -1;
                                                                    });
        const rowData = [ ...techSpecNullData , ...dataWithTechSpecData ] ;                                                           
        return(
            <ReactGrid gridRowData={rowData} gridColData={headData} rowClassRules={{ allowDangerTag:true }} 
            paginationPrefixId={localConstant.paginationPrefixIds.dashboardAssignmentGridId}
             />
        );
    }
}

Assignments.prototypes = {
    headData:PropTypes.array.isrequired,
    rowData:PropTypes.array.isrequired
};
Assignments.defaultprops ={
    headData:[],
    assignmentGridData:null
};

export default Assignments;