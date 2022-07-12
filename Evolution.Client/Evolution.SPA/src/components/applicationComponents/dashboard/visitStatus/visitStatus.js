import React,{ Component } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { headerData } from './visitStatusHeader';
import dateUtil from '../../../../utils/dateUtil';
import PropTypes from 'proptypes';

import { getlocalizeData, isEmpty } from '../../../../utils/commonUtils';

const localConstant = getlocalizeData();

class VisitStatus extends Component{
    componentDidMount() {
      this.props.actions.FetchVisitStatus();
    }
    render(){   
        const headData = headerData;
        const defaultSort = [
            { "colId": "techSpecialists",
                "sort": "asc" },
        ];
        this.props.visitStatusGridData && this.props.visitStatusGridData.map(visit => {
            if (visit.visitStatus === "A")
                visit.visitStatus = "Approved By Contract Holder";
            if (visit.visitStatus === "C")
                visit.visitStatus = "Awaiting Approval";
            if (visit.visitStatus === "J")
                visit.visitStatus = "Rejected By Operator";
            if (visit.visitStatus === "O")
                visit.visitStatus = "Approved By Operator";
            if (visit.visitStatus === "Q")
                visit.visitStatus = "Confirmed - Awaiting Visit";
            if (visit.visitStatus === "R")
                visit.visitStatus = "Rejected By Contract Holder";
            if (visit.visitStatus === "T")
                visit.visitStatus = "Tentative -  Pending Approval";
            if (visit.visitStatus === "U")
                visit.visitStatus = "TBA - Date Unknown";
            if (visit.visitStatus === "D")
                visit.visitStatus = "Unused Visit";
       
                const technicalSpecialists = [];
                if(Array.isArray(visit.techSpecialists) && visit.techSpecialists.length>0){
                    visit.techSpecialists.map((iterated)=>{                        
                        technicalSpecialists.push(iterated.fullName);
                    });
                    visit.techSpecialists = (technicalSpecialists).join(',');
                }

            }); 
        const techSpecNullData = this.props.visitStatusGridData.filter(item => isEmpty(item.techSpecialists)
            || isEmpty(item.visitOperatingCompanyCoordinator))
            .sort(function (a, b) {

                if (a.visitId > b.visitId)
                    return 1;
                if (b.visitId > a.visitId)
                    return -1;
                if (a.visitCustomerName > b.visitCustomerName)
                    return 1;
                if (b.visitCustomerName > a.visitCustomerName)
                    return -1;
            });
        const dataWithTechSpecData = this.props.visitStatusGridData.filter(item => !isEmpty(item.techSpecialists) && item.visitStatus !== "Unused Visit"
            && !isEmpty(item.visitOperatingCompanyCoordinator))
            .sort(function (a, b) {

                if (a.visitId > b.visitId)
                    return 1;
                if (b.visitId > a.visitId)
                    return -1;
            });
        const rowData = [ ...techSpecNullData, ...dataWithTechSpecData ];
        return(
            <ReactGrid gridRowData={rowData} gridColData={headData} 
            //rowClassRules={{ allowDangerTag: true }} //Changes for DM issue(D725)
            columnPrioritySort={ defaultSort } 
            paginationPrefixId={localConstant.paginationPrefixIds.dashboardVisitStatusGridId}
            />
        );
    }
}
VisitStatus.prototypes = {
    headData:PropTypes.array.isrequired,
    visitStatusGridData:PropTypes.array.isrequired
};
VisitStatus.defaultprops ={
    headData:[],
    visitStatusGridData:null
};
export default VisitStatus;