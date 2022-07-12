import React, { Component,Fragment } from 'react';
import { HeaderData } from './headerData.js';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { getlocalizeData } from '../../../../utils/commonUtils';
import LabelwithValue from '../../../../common/baseComponents/customLabelwithValue';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import CustomModal from '../../../../common/baseComponents/customModal';
import arrayUtil from '../../../../utils/arrayUtil';
const localConstant = getlocalizeData();
class Contracts extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isOpen: false
        };
    }
    componentDidMount() {
        const tabInfo = this.props.tabInfo;
        /** 
         * Below check is used to avoid duplicate api call
         * the value to isTabRendered is set in customTabs on tabSelect event handler
        */
        if(tabInfo && tabInfo.componentRenderCount === 0){
            this.props.actions.FetchCompanyContract(this.props.selectedContractStatus);
        }
    }
    handleChangeStatus = (e) => {
        this.props.actions.FetchCompanyContract(e.target.value);
    }

    contractRefreshHandler = () => {
        this.props.actions.FetchCompanyContract(this.props.selectedContractStatus);
    };

    render() {
        const headData = HeaderData;
        const { companyContractDetail } = this.props;
        const rowData = companyContractDetail;
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        return (
            <Fragment>
                <CustomModal modalData={modelData} />
            <div className="genralDetailContainer customCard mt-0">
                <div className="row mb-0">
                    <div className="col s2 " >
                        <p className="bold">{localConstant.companyDetails.Contracts.CONTRACTS}</p>

                    </div>
                    <div className="offset-s6 col s4">
                        <LabelwithValue
                            className=" col s6 pr-2 mt-2 m6 right-align"
                            label={localConstant.companyDetails.Contracts.CONTRACT_STATUS}
                        />
                        <CustomInput
                            hasLabel={false}
                            divClassName='col s4 pl-0 m6'
                            type='select'
                            selectstatus={true}
                            className="browser-default"
                            optionsList={localConstant.commonConstants.statusAll}
                            optionName='name'
                            optionValue="value"
                            onSelectChange={this.handleChangeStatus}
                            defaultValue= {this.props.selectedContractStatus}
                            // disabled={this.props.interactionMode} //Commented for Defect 864
                        />
                    </div>
                </div>
                <div className="customCard">
                    <ReactGrid 
                        gridRowData={rowData} 
                        gridColData={headData}
                        gridRefreshHandler = { this.contractRefreshHandler }
                        paginationPrefixId={localConstant.paginationPrefixIds.companyContracts} />
                </div>
            </div>
        </Fragment>
        );

    }
}

export default Contracts;
