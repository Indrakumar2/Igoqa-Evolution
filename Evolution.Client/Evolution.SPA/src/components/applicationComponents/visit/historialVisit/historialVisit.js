import React, { Component } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './visitHeader';
import { getlocalizeData, formInputChangeHandler } from '../../../../utils/commonUtils';
import { visitTabDetails } from '../../../viewComponents/visit/visitDetails/visitTabsDetails';
import { isEmptyOrUndefine } from '../../../../utils/commonUtils';

const localConstant = getlocalizeData();

class HistorialVisit extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isPanelOpen: true
        };
        this.updatedData = {};
    }

    RefereshHistoricalVisits = (e) => {        
        const isNewVisit = (this.props.currentPage === localConstant.visit.EDIT_VIEW_VISIT_MODE ? false : true);        
        this.props.actions.FetchHistoricalVisit(isNewVisit);
    };
  
    //All Input Handle get Name and Value
    inputHandleChange = (e) => {
        e.preventDefault();
        const inputvalue = formInputChangeHandler(e);
        this.updatedData[inputvalue.name] = inputvalue.value;
    }

    componentDidMount() {
        //if(this.isPageRefresh()) {
        // const isNewVisit = (this.props.currentPage === localConstant.visit.EDIT_VIEW_VISIT_MODE ? false : true);
        // this.props.actions.FetchHistoricalVisit(isNewVisit);
        //}
    };

    isPageRefresh() {
        let isRefresh = true;
        visitTabDetails.forEach(row => {
            if(row["tabBody"] === "HistorialVisit") {
                isRefresh = row["isRefresh"];
                row["isRefresh"] = false;
                row["isCurrentTab"] = true;
            } else {
                row["isCurrentTab"] = false;
            }
        });
        return isRefresh;
    }   

    getHistoricalVisitList() {
        if(isEmptyOrUndefine(this.props.historicalVisits)) {
            return [];
        } else if(this.props.currentPage === localConstant.visit.EDIT_VIEW_VISIT_MODE) {
            return this.props.historicalVisits.filter(x => [ 'A','C','J','O','R','D' ].includes(x.visitStatus) && (x.visitId !== parseInt(this.props.visitInfo.visitId)));
        } else {
            return this.props.historicalVisits.filter(x => [ 'A','C','J','O','R','D' ].includes(x.visitStatus));
        }     
    }

    render() {
        return (
            <div className="customCard">                    
                 <h6 className="bold">{localConstant.visit.HISTORICAL_VISITS} <a onClick={this.RefereshHistoricalVisits} className="right mt-0 mr-2 btn-small bold" ><i className="zmdi zmdi-refresh zmdi-hc-lg"></i></a></h6>                 
                <ReactGrid gridRowData={this.getHistoricalVisitList()} gridColData={HeaderData}
                    paginationPrefixId={localConstant.paginationPrefixIds.visitHistoricalId} />
            </div>
        );
    }
}

export default HistorialVisit;
