import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { AppMainRoutes } from '../../../../../routes/routesConfig';
import { ObjectIntoQuerySting } from '../../../../../utils/commonUtils';

class  MySearchAnchor extends Component{

    selectedRowHandler = () => {
        let redirectionURL = "";
        let searchType = "";
        if (this.props.data) {
            if (this.props.data.searchType === "Quick") {
                redirectionURL = AppMainRoutes.quickSearch;
                searchType = 'Quick';
            }
            else if (this.props.data.searchType === "PreAssignment") {
                redirectionURL = AppMainRoutes.preAssignment;
                searchType = 'PreAssignment';
            }
        }
        const preAssignmentId = this.props.data.id;
        const queryObj = {
            searchType: searchType,
            preID: preAssignmentId
        };
        const queryStr = ObjectIntoQuerySting(queryObj);
        this.openMySearch(redirectionURL, queryStr);
    };

    openMySearch = (redirectionURL, queryStr) => {
        window.open(redirectionURL + '?' + queryStr, '_blank');
    };

    render(){
        const preAssignmentId = this.props.data.id;
        return(
            <a  href='javascript:void(0)' onClick={(e) => this.selectedRowHandler()} className="link" >{ preAssignmentId }</a>
            // <Link target="_blank" to={{ pathname:redirectionURL, search:`?searchType=${ searchType }&preID=${ this.props.data.id }` }} onClick={this.selectedRowHandler}  className="link">{preAssignmentId}</Link>
        );
    }
};

export default MySearchAnchor;