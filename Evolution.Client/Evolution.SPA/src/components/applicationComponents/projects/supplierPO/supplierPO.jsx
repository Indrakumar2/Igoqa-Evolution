import React, { Component, Fragment } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './headerData';
import CustomModal from '../../../../common/baseComponents/customModal';
import { getlocalizeData } from '../../../../utils/commonUtils';
const localConstant = getlocalizeData();

class SupplierPO extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isOpen: false
        };
    }
    componentDidMount = () => {
        const tabInfo = this.props.tabInfo;
        /** 
         * Below check is used to avoid duplicate api call
         * the value to isTabRendered is set in customTabs on tabSelect event handler
        */
        if(tabInfo && tabInfo.componentRenderCount === 0)
            this.props.actions.FetchSupplierPo();
    }

    supplierPORefreshHandler = () => {
        this.props.actions.FetchSupplierPo();
    };

    render() {
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        return (
            <Fragment>
                <CustomModal modalData={modelData} />
                <div className="genralDetailContainer customCard mt-0">
                    <div className="row mb-0">
                        <div className="col s4 " >
                            <p className="bold">{localConstant.project.SUPPLIER_PO}</p>
                        </div>
                    </div>
                    <div className="customCard">
                        <ReactGrid 
                            gridRowData={this.props.supplierPoData} 
                            gridColData={HeaderData}
                            paginationPrefixId={localConstant.paginationPrefixIds.projectSupplierPO}
                            gridRefreshHandler = { this.supplierPORefreshHandler } />
                    </div>
                </div>
            </Fragment>
        );
    }
}

export default SupplierPO;