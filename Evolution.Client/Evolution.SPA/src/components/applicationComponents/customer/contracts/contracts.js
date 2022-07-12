import React, { Component ,Fragment } from 'react';
import { HeaderData } from './headerData.js';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { getlocalizeData } from '../../../../utils/commonUtils';
import LabelwithValue from '../../../../common/baseComponents/customLabelwithValue';
import CustomInput from '../../../../common/baseComponents/inputControlls';
const localConstant = getlocalizeData();

class Contracts extends Component {
    componentDidMount() {
        const tabInfo = this.props.tabInfo;
        /** 
         * Below check is used to avoid duplicate api call
         * the value to isTabRendered is set in customTabs on tabSelect event handler
        */
        if(tabInfo && tabInfo.componentRenderCount === 0){
            this.props.actions.FetchCustomerContracts(this.props.selectedcontractStatusValue);
        }
    }

    handleChangeStatus = (e) => {
        this.props.actions.FetchCustomerContracts(e.target.value);
    }

    contractRefreshHandler = () => {
        this.props.actions.FetchCustomerContracts(this.props.selectedcontractStatusValue);
    };

    render() {
        const headData = HeaderData.ContractHeader;
        const rowData = this.props.customerContractDetail;
        return (
            <Fragment>
            <div className="customCard mt-0">
                <div className="row mb-0">
                    <div className="col s2 " >
                    <h6 className="col s12 label-bold mt-2 mb-0 pl-0">{localConstant.companyDetails.Contracts.CONTRACTS}</h6>

                    </div>
                    <div className="offset-s6 col s4">
                        <LabelwithValue
                            className=" col s6 pr-2 mt-0 m6 right-align"
                            label={localConstant.companyDetails.Contracts.CONTRACT_STATUS}
                        />
                        <CustomInput
                            hasLabel={false}
                            divClassName='col s4 pl-0 m6'
                            type='select'
                            className="browser-default"
                            optionsList={localConstant.commonConstants.statusAll}
                            optionName='name'
                            optionValue="value"
                            onSelectChange={this.handleChangeStatus}
                            defaultValue={this.props.selectedcontractStatusValue}
                            // disabled={this.props.interactionMode} //Commented for Defect 864
                        />
                    </div>
                </div>
                <div className="customCard">
                    <ReactGrid 
                        gridRowData={rowData} 
                        gridColData={headData}
                        paginationPrefixId={localConstant.paginationPrefixIds.customerContracts}
                        gridRefreshHandler = { this.contractRefreshHandler } />
                </div>
            </div>
        </Fragment>
        );
    }
}
export default Contracts;