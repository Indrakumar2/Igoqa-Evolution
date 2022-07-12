import React, { Component } from 'react';
import { headerData } from './headerData';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { getlocalizeData, isEmpty } from '../../../../utils/commonUtils';
import SearchFilter from '../../../searchFilter';
import { searchFilterWithFunctionRefs ,defaultSupplierSearchField } from './supplierpoSearchFields';
import { AppMainRoutes } from '../../../../routes/routesConfig';
import moment from 'moment';
import { ReplaceString } from '../../../../utils/stringUtil';
import { defaultFilterData } from '../../assignment/assignmentSearch/assignmentSearchFields';
import { applicationConstants } from '../../../../constants/appConstants';
import Modal from '../../../../common/baseComponents/modal';
import ErrorList from '../../../../common/baseComponents/errorList';
import { validateObjectHasValue } from '../../../../utils/objectUtil';

const localConstant = getlocalizeData();

class supplierpoSearch extends Component {
    constructor(props) {
        super(props);
        this.childSearchFilter = React.createRef();
        this.callBackFuncs ={
            onReset:()=>{}
          };
          this.callBackFunc ={
            onReset:()=>{}
          };
        const functionRefs = {};
        functionRefs["changeHandler"] = this.changeHandler;
        functionRefs["fetchMainSupplier"] = this.fetchMainSupplier;
        functionRefs["getSelectedMainSupplier"] = this.getSelectedMainSupplier;
        functionRefs["getSelectedSubSupplier"] = this.getSelectedSubSupplier;
        functionRefs["completedDateChange"] = this.completedDateChange;
        functionRefs["deliveryDateChange"] = this.deliveryDateChange;
        functionRefs["supplierPopupOpen"] = this.supplierPopupOpen;
        functionRefs["clearSearchRef"] = this.callBackFuncs;
        functionRefs["clearSearchSubSupplierRef"] = this.callBackFunc;
        functionRefs["DocMasterData"]=this.DocMasterData;
        functionRefs["resetSupplierpo"] = this.resetSupplierpo;
        functionRefs["csvExportClick"] = this.csvExportClick;
        this.searchFields = searchFilterWithFunctionRefs(functionRefs);
        this.state = {
           searchFieldData :defaultSupplierSearchField,
           supplierPOSearchList:[], //Search Optimization
           supplierPOSearchTotalCount:0,
           hasGlobalViewAccess:false,
           searchFields:this.searchFields,
           errorList:[]
        };
        this.supplierSearchFilter = {
            supplierPOStatus: 'O',
        };
        this.hasGlobalAccess = [];
        this.selectCompanyData=[];
        this.csvLink = React.createRef();

        this.modelBtns = {
            errorListButton: [
              {
                type:'button',
                name: localConstant.commonConstants.OK,
                action: (e) => this.closeErrorList(e),
                btnID: "closeErrorList",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true
              }
            ]
        };
    }

    componentDidMount() {
        this.props.actions.FetchDocumentTypeMasterData();
        this.props.actions.ClearData();//D648 CustomerName Retained

        if (Array.isArray(this.props.companyList) && this.props.companyList.length > 0 && this.props.selectedCompany) {
            this.selectCompanyData = this.props.companyList.filter(x => x.companyCode == this.props.selectedCompany);
        }

        if (Array.isArray(this.props.activities) && this.props.activities.length > 0) {
            this.hasGlobalAccess = this.props.activities.filter(activity => activity.activity === "SU0003");
            if (Array.isArray(this.hasGlobalAccess) && this.hasGlobalAccess.length === 0) {
                if (this.searchFields) {
                    if (Array.isArray(this.searchFields.subComponents)) {
                        let isChanged = false;
                        this.searchFields.subComponents.forEach(componentProps => {
                            if (componentProps.properties && componentProps.properties.name === 'supplierPOContractHolderCompany') {
                                componentProps.properties.disabled = true;
                                isChanged = true;
                                return true;
                            }
                        });
                        if (isChanged) {
                            const updateFilter = { ...this.state.searchFieldData };
                            if (Array.isArray(this.selectCompanyData) && this.selectCompanyData.length > 0)
                                updateFilter.supplierPOContractHolderCompany = this.selectCompanyData[0].id;
                            this.setState({
                                searchFields: this.searchFields,
                                searchFieldData: updateFilter
                            });
                        }
                    }
                }
            }
        }
    }
    componentWillUnmount() {
        this.props.actions.ClearData();
    }
    /*Start -- Code added for supplier Po Search
             To add header name     
        */
    getPanelSubTitle = () => {
        if (this.props.location.pathname === AppMainRoutes.editSupplierPO) {
            return localConstant.supplierpo.EDIT_VIEW_SUPPLIER_PO;
        }
    }
    /*End -- Code added for supplier PO module*/

    changeHandler = (e) => {
            const updateFilter = { ...this.state.searchFieldData }; 
            updateFilter[e.target.name]=e.target.value;           
            this.setState({ searchFieldData:updateFilter });
    }
    fetchMainSupplier = (data) => {
        const params = {
            supplierName: data.serachInput,
            country: data.selectedCountry
        };
        this.props.actions.FetchSupplierSearchList(params);
    }
    getSelectedMainSupplier = (data) => {
        const filterData = { ...this.state.searchFieldData };
              filterData.supplierPOMainSupplierName = data && Array.isArray(data) && data.length > 0 && data[0].supplierName;
              filterData.SupplierPOMainSupplierId=data && Array.isArray(data) && data.length > 0 && data[0].supplierId;
            // Added for Defect 866 Starts
              this.setState({
                searchFieldData: filterData
            });
            // Added for Defect 866 End
    }
    getSelectedSubSupplier = (data) => {
        const filterData = { ...this.state.searchFieldData };
        filterData.supplierPOSubSupplierName = data && Array.isArray(data) && data.length > 0 && data[0].supplierName;
        filterData.SupplierPOSubSupplierId =data && Array.isArray(data) && data.length > 0 && data[0].supplierId;
        // Added for Defect 866 Starts
        this.setState({
            searchFieldData: filterData
        });
        // Added for Defect 866 End
    }

    completedDateChange = (date) => {
        const filterData = { ...this.state.searchFieldData };
        if (isEmpty(date)) {
            filterData.supplierPOCompletedDate = "";
        }
        else {
             filterData.supplierPOCompletedDate = moment(date);           
        }
        this.setState({
            searchFieldData: filterData
        });
    }
    deliveryDateChange = (date) => {
        const filterData = { ...this.state.searchFieldData };
        if (isEmpty(date)) {
            filterData.supplierPODeliveryDate = "";            
        }
        else {
            filterData.supplierPODeliveryDate = moment(date);           
        }
        this.setState({
            searchFieldData: filterData
            });
    }
    supplierPopupOpen = (data) => {
        this.props.actions.ClearSupplierSearchList();
    }
    //searchSupplierpo search
    searchSupplierpo = (e,isLoadMore) => {
        e.preventDefault();
        const SearchFilter = { ...this.state.searchFieldData };
        SearchFilter.SupplierPOCustomerName = this.props.defaultCustomerName
            ? this.props.defaultCustomerName.trim()
            : '';
        SearchFilter.customerId = this.props.defaultCustomerId
            ? this.props.defaultCustomerId
            : '';      
        if (!SearchFilter.supplierPOStatus) {
            SearchFilter.supplierPOStatus = this.supplierSearchFilter.supplierPOStatus;
        }
        if (!SearchFilter.searchDocumentType) {
            SearchFilter.searchDocumentType = this.supplierSearchFilter.searchDocumentType;
        }
        if(!isEmpty(SearchFilter.supplierPOCompletedDate)){
            SearchFilter.supplierPOCompletedDate=moment(SearchFilter.supplierPOCompletedDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        } 
        if(!isEmpty(SearchFilter.supplierPODeliveryDate)){
            SearchFilter.supplierPODeliveryDate=moment(SearchFilter.supplierPODeliveryDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        } 
        
        //search optimization
        if(isLoadMore){
            SearchFilter.offSet=this.state.supplierPOSearchList?this.state.supplierPOSearchList.length:0;
            SearchFilter.supplierPOSearchTotalCount=this.state.supplierPOSearchTotalCount;
        }

        if (Array.isArray(this.hasGlobalAccess) && this.hasGlobalAccess.length === 0) {
            if (Array.isArray(this.selectCompanyData) && this.selectCompanyData.length > 0)
                SearchFilter.supplierPOCompanyId = this.selectCompanyData[0].id;
        }
        const isValid = validateObjectHasValue(SearchFilter, Object.keys(applicationConstants.supplierPOSearchValidateFields));
        if(!isValid){
            this.setState({
                errorList: Object.values(applicationConstants.supplierPOSearchValidateFields)
            });
            return false;
        }

        const $this = this;
        this.props.actions.FetchSupplierPOSearchResults(SearchFilter).then((res) => {
            if (res) {
                if (!isLoadMore) {
                    this.setState({
                        supplierPOSearchTotalCount: res.recordCount,
                        supplierPOSearchList: [],
                        //isPanelOpen: !this.state.isPanelOpen
                    });
                    $this.childSearchFilter.current.panelClick();
                }
                const tempData = Object.assign([], this.state.supplierPOSearchList);
                if (Array.isArray(this.props.supplierPOSearchData) && this.props.supplierPOSearchData.length > 0)
                    tempData.push(...this.props.supplierPOSearchData);
                this.setState({
                    supplierPOSearchList: tempData
                });
            }
        });
    }

    csvExportClick = async (event, done) => {
        event.preventDefault();
        const SearchFilter = { ...this.state.searchFieldData };
        SearchFilter.isExport = true;
        SearchFilter.SupplierPOCustomerName = this.props.defaultCustomerName
            ? this.props.defaultCustomerName.trim()
            : '';
        SearchFilter.customerId = this.props.defaultCustomerId
            ? this.props.defaultCustomerId
            : '';      
        if (!SearchFilter.supplierPOStatus) {
            SearchFilter.supplierPOStatus = this.supplierSearchFilter.supplierPOStatus;
        }
        if (!SearchFilter.searchDocumentType) {
            SearchFilter.searchDocumentType = this.supplierSearchFilter.searchDocumentType;
        }
        if(!isEmpty(SearchFilter.supplierPOCompletedDate)){
            SearchFilter.supplierPOCompletedDate=moment(SearchFilter.supplierPOCompletedDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        } 
        if(!isEmpty(SearchFilter.supplierPODeliveryDate)){
            SearchFilter.supplierPODeliveryDate=moment(SearchFilter.supplierPODeliveryDate).format(localConstant.commonConstants.UI_DATE_FORMAT).toString();
        } 
        
        this.props.actions.FetchSupplierPOSearchResults(SearchFilter).then((res) => {
            if (res && res.result && res.result.baseSupplierPO && res.result.header) {
                // this.setState({
                //     headers:res.headers
                // });
                // this.headers=res.headers;
                this.csvLink.link.click();
            }
        });
    }

    loadMoreClick=(data)=>{
        this.searchSupplierpo(data,true);
    }

    resetSupplierpo = (e) => {
        const resetField = { ...defaultSupplierSearchField };
        if (Array.isArray(this.hasGlobalAccess) && this.hasGlobalAccess.length === 0) {
            if (Array.isArray(this.selectCompanyData) && this.selectCompanyData.length > 0)
                resetField.supplierPOContractHolderCompany = this.selectCompanyData[0].id;
        }
        this.setState({
            searchFieldData: resetField,
            supplierPOSearchTotalCount: 0,
            supplierPOSearchList: [],
        });
        this.props.actions.ClearSupplierpoSearchResults();
        this.props.actions.ClearSupplierSearchList();
        this.props.actions.ClearData();
        this.callBackFuncs.onReset();
        this.callBackFunc.onReset();

    }
    componentWillUnmount = () => {
        //this.SearchFilter = {};
        this.props.actions.ClearSupplierpoSearchResults();
    }

    componentWillMount = () => {
        //this.SearchFilter = {};
        this.props.actions.ClearSupplierpoSearchResults();
    }
    closeErrorList = (e) => {
        e.preventDefault();
        this.setState({
            errorList: []
        });
    }

    render() {
        const subTitle = this.getPanelSubTitle();
        const { supplierList,documentTypesData,companyList } = this.props;
        const supplierPOStatus = localConstant.commonConstants.supplierPOStatusAll;
        const supplierPOCompletedDate = this.state.selectedCompletedDate;
        const supplierPODeliveryDate = this.state.selectedDeliveryDate;
        return (
            <div className="customCard">
                <SearchFilter colSize="s12"
                    isArrow={true}
                    heading={localConstant.supplierpo.SEARCH_SUPPLIER_PO}
                    subtitle={` : ${ subTitle }`}
                    searchFields={this.state.searchFields}
                    isSupplierPo={true}
                    filterFieldsData={{
                        supplierList,
                        supplierPOStatus,
                        supplierPOCompletedDate,
                        supplierPODeliveryDate,
                        documentTypesData,
                        companyList,
                        // csvData:this.props.supplierPOSearchData&&this.props.supplierPOSearchData.baseSupplierPO?this.props.supplierPOSearchData.baseSupplierPO:[], 
                        // csvHeader:this.props.supplierPOSearchData&&this.props.supplierPOSearchData.header?this.props.supplierPOSearchData.header:[],  
                    }}
                    // setData={{
                    //     supplierPOStatus: this.supplierSearchFilter.supplierPOStatus
                    // }}
                    setData={
                        this.state.searchFieldData
                    }
                    resetHandler={(e) => this.resetSupplierpo(e)}
                    submitHandler={(e) => this.searchSupplierpo(e)}
                    ref={this.childSearchFilter}
                    csvRef={(r) => this.csvLink = r}
                />
                <ReactGrid
                    gridRowData={this.state.supplierPOSearchList}
                    gridColData={headerData.supplierPoSearch}
                    loadMoreClick={this.loadMoreClick}
                    totalCount={this.state.supplierPOSearchTotalCount}
                />

<Modal title={localConstant.commonConstants.CHECK_ANY_MANDATORY_FIELD}
                    titleClassName="chargeTypeOption"
                    modalContentClass="extranetModalContent"
                    modalId="errorListPopup"
                    formId="errorListForm"
                    buttons={this.modelBtns.errorListButton}
                    isShowModal={this.state.errorList.length > 0}>
                    <ErrorList errors={this.state.errorList} />
                </Modal> 
            </div>
        );
    }
}

export default supplierpoSearch;
supplierpoSearch.propTypes = {
};

supplierpoSearch.propTypes = {

};