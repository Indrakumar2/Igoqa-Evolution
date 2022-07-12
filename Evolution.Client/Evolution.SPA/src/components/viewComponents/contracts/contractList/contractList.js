import React, { Component, Fragment } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { ContractHeaderData } from '../contractHeaderData.js';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import Panel from '../../../../common/baseComponents/panel';
import { getlocalizeData } from '../../../../utils/commonUtils';
import CustomerAndCountrySearch from '../../../applicationComponents/customerAndCountrySearch';
import Title from '../../../../common/baseComponents/pageTitle';
import { AppMainRoutes } from '../../../../routes/routesConfig';
const localConstant = getlocalizeData();
const defaultSearchField={
                contractNumber:'',
                customerContractNumber:'',
                contractStatus: 'O',
                contractHoldingCompany:'',
                selectedCompany:'',
                searchDocumentType:'',
                documentSearchText:''
};
const SearchFilterDiv = (props) => {

    return( 
    <form id="contractSearch" onSubmit={props.customerContractSearch}  autoComplete="off">
    <div className="row mb-0">
        <CustomerAndCountrySearch divClassName="s4 pr-0" isSupplier={true} />

        <CustomInput hasLabel={true}
            divClassName='col'
            label={localConstant.contract.CONTRACT_NO}
            onValueChange={props.handlerChange}
            type='text'
            colSize='s4 pl-0'
            inputClass="customInputs"
            name='contractNumber'
            dataValType='valueText'
            value={props.searchFieldData.contractNumber}
            />

        <CustomInput hasLabel={true}
            divClassName='col'
            label={localConstant.contract.CUSTOMER_CONTRACT_NUMBER}
            onValueChange={props.handlerChange}
            type='text'
            colSize='s4 pl-0'
            inputClass="customInputs"
            name='customerContractNumber'
            maxLength={50}
            dataValType='valueText'
            value={props.searchFieldData.customerContractNumber}/>

         {props.activities.filter(x=>x.activity==="C00001").length>0?<CustomInput hasLabel={true}
            name='contractHoldingCompany'
            divClassName='col'
            label={localConstant.contract.CONTRACT_HOLDING_COMPANY}
            type='select'
            colSize={props.projectMode ===  AppMainRoutes.searchContract ? 's4 hide' : 's4'}
            inputClass="customInputs"
            optionsList={props.contractHoldingCompany}
            optionName='companyName'
            optionValue="companyCode"
            disabled={false}
            className="browser-default"
            onSelectChange={props.handlerChange}
            //defaultValue={props.currentPage === "Edit Contract" ? '' : props.searchFieldData.contractHoldingCompany}
            defaultValue={props.searchFieldData.contractHoldingCompany} />:
            <CustomInput hasLabel={true}
            name='contractHoldingCompany'
            divClassName='col'
            label={localConstant.contract.CONTRACT_HOLDING_COMPANY}
            type='select'
            colSize={props.projectMode === AppMainRoutes.searchContract ? 's4 hide' : 's4'}
            inputClass="customInputs"
            optionsList={props.contractHoldingCompany}
            optionName='companyName'
            optionValue="companyCode"
            disabled={true}
            className="browser-default"
            onSelectChange={props.handlerChange}
            defaultValue={ props.selectedCompany} />}
            
            <CustomInput hasLabel={true}
                divClassName='col'
                onSelectChange={props.handlerChange}
                label={localConstant.contract.STATUS}
                name='contractStatus'
                type='select'
                colSize={props.projectMode ===  AppMainRoutes.searchContract? 's4 ' : 's4 pl-0'}
                optionSelecetLabel='All'
                optionsList={props.status}
                defaultValue={props.searchFieldData.contractStatus}
                optionName='name'
                optionValue="value"
                className="browser-default"
                disabled={false}                    
                
                />
    </div>
    <div className="row mb-0">
        <CustomInput hasLabel={true}
            divClassName='col'
            label={localConstant.contract.SEARCH_DOCUMENTS}
            type='select'
            colSize='s4'
            name="searchDocumentType"
            id="searchDocumentType"
            className="browser-default"
            defaultValue={props.searchFieldData.searchDocumentType}
            optionName="name"
            optionValue="name"
            optionsList={props.documentMasterData}
            onSelectChange={props.handlerChange}        
            disabled={false}
  />
        <CustomInput hasLabel={true}
            divClassName='col'
            label={localConstant.contract.SEARCH_TEXT}
            type='text' 
            dataValType='valueText'
            colSize='s4 pl-0'
            className="browser-default"
            inputClass="customInputs"
            onValueChange={props.handlerChange}
            value={props.searchFieldData.documentSearchText}
            name="documentSearchText"
            id="documentSearchText"
            maxLength={200} />
        <button type="submit" className="mt-4x mr-2 modal-close waves-effect waves-green btn ">{ localConstant.commonConstants.BTN_SEARCH }</button>
    <button type="button" onClick={props.clearSearchData} className="mt-4x modal-close waves-effect waves-green btn" >{localConstant.commonConstants.BTN_RESET}</button>
    </div>
</form>
);
};   

class ContractList extends Component {
    constructor(props) {
        super(props);
        this.state = {
            subTitle: 'Edit Contract',
            inputCtrlCustomerName: '',
            contractStatus: 'O',
            searchFieldData:defaultSearchField

        };
        this.isFromSearchContract = false;
        this.updatedData = {};
        this.selectedcustomerName = [];

    }
    /*Start -- Code added for project module 
               To add header name     
    */
    getPanelSubTitle = () => {
        if (this.props.location.pathname === AppMainRoutes.projectSearchContract) {
            this.isFromSearchContract = true;           
            return ': ' + localConstant.project.ADD_PROJECT;
        } else {
            this.isFromSearchContract = false;
            return ' : Edit/View Contract';
        }
    }
    /*End -- Code added for project module*/

    componentDidMount() {
        this.getCurrentPathUrl();
        const defaultField= defaultSearchField;
        defaultField.selectedCompany = this.props.selectedCompany;
        this.setState({ searchFieldData:defaultField });
        //this.updatedData.contractStatus = this.props.contractStatus;
        this.props.actions.ClearSearchData();
        if (this.props.location.pathname === AppMainRoutes.searchContract) {
            if(!this.props.isPanelOpen){
               this.props.actions.ShowHidePanel();
               this.props.actions.FetchDocumentTypeMasterData();
            }
        }
    }
    panelClick = (e) => {
        this.props.actions.ShowHidePanel();
    }
    handlerChange = (e) => {    
        const searchData={ ...this.state.searchFieldData }; 
        searchData[e.target.name] = e.target.value; 
        searchData["customerName"] = this.props.defaultCustomerName.customerName;
        //this.updatedData.customerName = this.props.defaultCustomerName.customerName;
        //this.updatedData[e.target.name] = e.target.value;
        //const searchData = { ...this.state.searchFieldData, ...this.updatedData };
        this.setState({ searchFieldData:searchData });
    }

    //Clear Search Data
    clearSearchData = () => {       
        this.setState({ searchFieldData:defaultSearchField });      
        //this.updatedData = {};
        //this.updatedData.contractStatus = this.state.searchFieldData.contractStatus;
        this.props.actions.ClearSearchData();
    }
    //Contract Main Search 
    customerContractSearch = (e) => {
        e.preventDefault();
        const SearchContractList = { ...this.state.searchFieldData };
        const customerName = this.props.defaultCustomerName;
        
        // this.updatedData.contractHoldingCompany = (this.props.projectMode === "createProject") ? this.props.selectedCompany : this.updatedData.contractHoldingCompany;
        // this.updatedData.customerName = customerName.trim();
        if(this.props.defaultCustomerId){
            SearchContractList.customerName = customerName.trim().replace(/&/g, "%26");
            SearchContractList.customerId = this.props.defaultCustomerId;
        }
        if(this.props.location.pathname === AppMainRoutes.searchContract){
            SearchContractList.contractHoldingCompany = this.props.selectedCompany;
        }
        if(!this.props.activities.filter(x=>x.activity==="C00001").length>0){
            SearchContractList.contractHoldingCompany = this.props.selectedCompany;
        }
        
        SearchContractList["isFromSearchContract"] = this.isFromSearchContract;
        SearchContractList.contractNumber=encodeURIComponent(SearchContractList.contractNumber);
        SearchContractList.customerContractNumber = encodeURIComponent(SearchContractList.customerContractNumber);
        this.props.actions.FetchCustomerContract(SearchContractList);
        this.props.actions.ShowHidePanel();
    }

    //Get Current URL Path
    getCurrentPathUrl = () => {
        if (this.props.location.pathname === AppMainRoutes.editContracts) {
            this.props.actions.GetCurrentPage('Edit Contract');
            this.props.actions.UpdateInteractionMode(false);
        } else if (this.props.location.pathname === AppMainRoutes.viewContracts) {
            this.props.actions.GetCurrentPage('View Contract');
            this.props.actions.UpdateInteractionMode(true);
        }
        this.getPanelSubTitle();
    }
    hideModal = (e) => {
        e.preventDefault();
        this.props.actions.ContractHideModal();
        //this.updatedData = {};
    }
    // componentWillUnmount = () => {
    //     this.clearSearchData();
    // }
    render() {
        const { documentTypesData }=this.props;
        const masterDocData=documentTypesData&&documentTypesData.filter(x=>x.moduleName==="Contract");
        const defaultContractSortOrder =[
            { "colId": "contractNumber",
              "sort": "asc" }
        ];
        return (
            <Fragment>
                <div className="companyPageContainer customCard">
                    <Panel colSize="s12"
                        isArrow={true}
                        heading="Search Contract"
                        subtitle={this.getPanelSubTitle()}
                        onpanelClick={this.panelClick}
                        isopen={this.props.isPanelOpen} >
                        <SearchFilterDiv
                            handlerChange={this.handlerChange}
                            handlerFocus={this.handlerFocus}
                            selectCustomerSearch={this.selectCustomerSearch}
                            clearSearchData={this.clearSearchData}
                            searchFieldData = {this.state.searchFieldData}
                            searchFilter={this.searchFilter}
                            interactionMode={this.props.interactionMode}
                            contractHoldingCompany={this.props.contractHoldingCompany}
                            customerContractSearch={(e)=>this.customerContractSearch(e)}
                            status={localConstant.commonConstants.status}
                            contractStatus={this.props.contractStatus}
                            currentPage={this.props.currentPage}
                            projectMode={this.props.location.pathname}
                            selectedCompany={this.props.selectedCompany}
                            documentMasterData={masterDocData} 
                            activities={this.props.activities} >
                        </SearchFilterDiv>
                    </Panel>
                    <ReactGrid gridRowData={this.props.customerContract} gridColData={ContractHeaderData} columnPrioritySort={defaultContractSortOrder}/>
                </div>

            </Fragment>
        );
    }
}

export default ContractList;
