import React, { Component } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './timeSheetHeader';
import PropTypes from 'proptypes';
import { getlocalizeData, isEmpty } from '../../../../utils/commonUtils';

const localConstant = getlocalizeData();

class Timesheet extends Component {
    componentDidMount() {
        this.props.actions.FetchTimesheetPendingAproval();
    }

    render() {
        const headData = HeaderData;
        const defaultSort = [
            { "colId": "techSpecialists",
                "sort": "asc" },
        ];
        this.props.timeSheetPendingAproval && this.props.timeSheetPendingAproval.map(timesheet => {
            
                const technicalSpecialists = [];
                if(Array.isArray(timesheet.techSpecialists) && timesheet.techSpecialists.length>0){
                    timesheet.techSpecialists.map((iterated)=>{                        
                        technicalSpecialists.push(iterated.fullName);
                    });
                    timesheet.techSpecialists = (technicalSpecialists).join(',');
                }
        });
               
        const techSpecNullData = this.props.timeSheetPendingAproval.filter(item => isEmpty(item.techSpecialists)
            || isEmpty(item.timesheetOperatingCoordinator))
            .sort(function (a, b) {

                if (a.timesheetId > b.timesheetId)
                    return 1;
                if (b.timesheetId > a.timesheetId)
                    return -1;
                if (a.timesheetCustomerName > b.timesheetCustomerName)
                    return 1;
                if (b.timesheetCustomerName > a.timesheetCustomerName)
                    return -1;
            });
        const dataWithTechSpecData = this.props.timeSheetPendingAproval.filter(item => !isEmpty(item.techSpecialists) && item.timesheetStatus !== 'E'
            && !isEmpty(item.timesheetOperatingCoordinator))
            .sort(function (a, b) {

                if (a.timesheetId > b.timesheetId)
                    return 1;
                if (b.timesheetId > a.timesheetId)
                    return -1;
            });
        const rowData = [ ...techSpecNullData, ...dataWithTechSpecData ];
               
        return (
            <ReactGrid gridRowData={rowData} gridColData={headData} rowClassRules={{ allowDangerTag: true }} 
            columnPrioritySort={ defaultSort }
            paginationPrefixId={localConstant.paginationPrefixIds.dashboardTimesheetGridId}
            />
        );
    }
}
Timesheet.prototypes = {
    headData:PropTypes.array.isrequired,
    timeSheetPendingAproval:PropTypes.array.isrequired
};
Timesheet.defaultprops ={
    headData:[],
    timeSheetPendingAproval:null
};
export default Timesheet;