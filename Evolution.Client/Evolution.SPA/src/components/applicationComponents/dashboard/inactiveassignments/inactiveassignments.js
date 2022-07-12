import React,{ Component } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { headerData } from './inactiveHeader';
import PropTypes from 'proptypes';
import { isEmpty } from '../../../../utils/commonUtils';
import { getlocalizeData } from '../../../../utils/commonUtils';

const localConstant = getlocalizeData();

class Inactiveassignments extends Component{
            componentDidMount() {
                const companyName = this.props.selectedCompany;
                this.props.actions.FetchInActiveAssignments(companyName);                 
            }
            render(){   
                
                this.props.inactiveAssignmentData && this.props.inactiveAssignmentData.map(inactiveAssignment => {                   
                        
                        const technicalSpecialists = [];
                        if(Array.isArray(inactiveAssignment.techSpecialists) && inactiveAssignment.techSpecialists.length>0){
                            inactiveAssignment.techSpecialists.forEach((iterated)=>{ 
                                if(iterated.fullName.trim()!=='')                      
                                technicalSpecialists.push(iterated.fullName);

                            });
                            inactiveAssignment.techSpecialists = (technicalSpecialists).join(',');
                        }
                }); 

                const techSpecNullData = this.props.inactiveAssignmentData.filter( item => isEmpty(item.techSpecialists) 
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
                const dataWithTechSpecData = this.props.inactiveAssignmentData.filter( item => !isEmpty(item.techSpecialists) 
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
                    <ReactGrid gridRowData={rowData} gridColData={headerData} rowClassRules={{ allowDangerTag: true }} 
                    paginationPrefixId={localConstant.paginationPrefixIds.dashboardInAssignmentGridId}
                   />
                );
            } 
}

Inactiveassignments.prototypes = {
    headData:PropTypes.array.isrequired,
    inactiveAssignmentData:PropTypes.array.isrequired
};
Inactiveassignments.defaultprops ={
    headData:[],
    inactiveAssignmentData:null
};

export default Inactiveassignments;