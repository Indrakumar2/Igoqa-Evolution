import React, { Component, Fragment } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './headerData';
import CustomModal from '../../../../common/baseComponents/customModal';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import { getlocalizeData } from '../../../../utils/commonUtils';
const localConstant = getlocalizeData();

class Assignments extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isOpen: false
        };
    };

    componentDidMount() {
        const tabInfo = this.props.tabInfo;
        /** 
         * Below check is used to avoid duplicate api call
         * the value to isTabRendered is set in customTabs on tabSelect event handler
        */
        if(tabInfo && tabInfo.componentRenderCount === 0){
            this.props.actions.FetchAssignmentSearchResults({ "assignmentSupplierPurchaseOrderId": this.props.supplierPOGenetalDetails.supplierPOId });
        }
    }

    assignmentRefreshHandler = () => {
        this.props.actions.FetchAssignmentSearchResults({ "assignmentSupplierPurchaseOrderId": this.props.supplierPOGenetalDetails.supplierPOId });
    };

    render() {
        const { supplierPOAssignmentData } = this.props;
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        return (
            <Fragment>
                <CustomModal modalData={modelData} />
                <div className="customCard">
                    <h6 className="bold col s3 pl-0 pr-3">{localConstant.supplierpo.ASSIGNMENT_SUPPLIER_PO}</h6>
                    <CardPanel className="white lighten-4 black-text" colSize="s12">
                        <ReactGrid 
                            gridRowData={supplierPOAssignmentData} 
                            gridColData={HeaderData}
                            paginationPrefixId={localConstant.paginationPrefixIds.supplierPOAssignments}
                            gridRefreshHandler = { this.assignmentRefreshHandler } />
                    </CardPanel>
                </div>
            </Fragment>
        );
    }
}

export default Assignments;