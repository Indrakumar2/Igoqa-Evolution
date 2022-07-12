import React,{ Component,Fragment } from 'react';
import Panel from '../../../../common/baseComponents/panel';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData,formInputChangeHandler,caseInsensitiveSort } from '../../../../utils/commonUtils';
import { HeaderData } from '../supplierHeaderData';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { ReplaceString } from '../../../../utils/stringUtil';

const localConstant=getlocalizeData();

const SupplierSearch = (props) => {
    return(
        <form id="supplierSearch" onSubmit={props.supplierSearch} onReset={props.clearSearchData} autoComplete="off">
            <div className="row mb-0">
                <CustomInput
                    hasLabel={true}
                    labelClass="customLabel"
                    label={localConstant.supplier.NAME}
                    divClassName="s12 m4"
                    type='text'
                    refProps='supplierNameId'
                    name="supplierName"
                    defaultValue=""
                    colSize='s12 m4'
                    inputClass="customInputs"
                    maxLength="200"
                    onValueChange={props.searchChangeHandler}
                />
                <CustomInput
                    hasLabel={true}
                    labelClass="customLabel"
                    label={localConstant.supplier.FULL_ADDRESS}
                    divClassName="s12 m4 pl-0"
                    type='text'
                    refProps='supplierAddressId'
                    name="supplierAddress"
                    defaultValue=""
                    colSize='s12 m4'
                    inputClass="customInputs"
                    maxLength="200"
                    onValueChange={props.searchChangeHandler}
                />
                <CustomInput
                    hasLabel={true}
                    labelClass="customLabel"
                    label={localConstant.supplier.POSTAL_CODE}
                    divClassName="s12 m4 pl-0"
                    type='text'
                    refProps='supplierPostalCodeId'
                    name="postalCode"
                    defaultValue=""
                    colSize='s12 m4'
                    inputClass="customInputs"
                    maxLength="200"
                    onValueChange={props.searchChangeHandler}
                />
                <CustomInput
                    hasLabel={true}
                    divClassName='col'
                    labelClass="customLabel"
                    name="country"
                    label={localConstant.supplier.COUNTRY}
                    type='select'
                    colSize='s12 m4'
                    defaultValue=""
                    className="browser-default customInputs"
                    optionsList={props.country}
                    optionName='name'
                    optionValue="id"
                    id="countryId"
                    onSelectChange={props.placeChangeHandler}
                />
                <CustomInput
                    hasLabel={true}
                    divClassName='col'
                    labelClass="customLabel"
                    name="state"
                    label={localConstant.supplier.STATE}
                    type='select'
                    colSize='s12 m4 pl-0'
                    defaultValue=""
                    className="browser-default customInputs"
                    optionsList={props.state}
                    optionName='name'
                    optionValue="id"
                    id="stateId"
                    disabled={ props.state.length > 0 ? false : true }
                    onSelectChange={props.placeChangeHandler}
                />
                <CustomInput
                    hasLabel={true}
                    divClassName='col'
                    labelClass="customLabel"
                    name="city"
                    label={localConstant.supplier.CITY}
                    type='select'
                    colSize='s12 m4 pl-0'
                    defaultValue=""
                    className="browser-default customInputs"
                    optionsList={props.city}
                    optionName='name'
                    optionValue="id"
                    id="cityId"
                    disabled={ props.city.length > 0 ? false : true } //Changes for ITK D1416
                    onSelectChange={props.placeChangeHandler}
                />
                <CustomInput hasLabel={true}
                divClassName='col'
                name="searchDocumentType"
                id="searchDocumentType"
                onSelectChange={props.searchChangeHandler}
                label={localConstant.supplier.SEARCH_DOCUMENT}
                type='select'
                colSize='s4'
                defaultValue=""
                className="browser-default"
                optionName="name"
                optionValue="value"               
                optionsList={props.documentMasterData}
                 />
                 <CustomInput 
                    hasLabel={true}
                    divClassName='col'
                    label={localConstant.supplier.SEARCH_TEXT}
                    type='text'
                    colSize='s12 m4 pl-0'
                    className="browser-default"
                    inputClass="customInputs"
                    onValueChange={props.searchChangeHandler}
                    name="documentSearchText"
                    id="documentSearchText"
                    maxLength={200}
                />
<button type="submit" className="mt-4x mr-2 modal-close waves-effect waves-green btn ">{localConstant.commonConstants.BTN_SEARCH}</button>
    <button type="reset" className="mt-4x modal-close waves-effect waves-green btn" >{localConstant.commonConstants.BTN_RESET}</button>
            </div>
            
        </form>
    );
};

class supplierSearch extends Component{

    constructor(props){
        super(props);
        this.state ={
            isPanelOpen:true,
        };
        this.updatedData = {};
    }

    componentDidMount(){
        this.clearSearchData();
    }

    supplierPanelClickHandler = () =>{
        this.setState((state)=>{
            return {
                isPanelOpen: !state.isPanelOpen
            };
        });
        // this.props.actions.SupplierSearchPanelState();
    }

    formInputChangeHandler = (e) => {
        const result = formInputChangeHandler(e);
        this.updatedData[result.name] = result.value.trim();
        if(result.name === "country" || result.name === "state" || result.name === "city"){ //Changes for D1536
            this.updatedData[e.target.id]=parseInt(result.value);
            this.updatedData[result.name]=e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent !== localConstant.commonConstants.SELECT ? e.nativeEvent.target[e.nativeEvent.target.selectedIndex].textContent : ""; 
        }
    }

    placeChangeHandler = (e) => {
        this.formInputChangeHandler(e);
        if(e.target.name === 'country' ){
            this.props.actions.SupplierFetchState(e.target.value);
            delete this.updatedData['state'];
            delete this.updatedData['city'];
        }
        if(e.target.name === 'state' ){
            this.props.actions.SupplierFetchCity(e.target.value);
            delete this.updatedData['city'];
        }
        if(e.target.name === 'searchDocumentType')
        {
            this.updatedData=e.target.name;
        }
        if(e.target.name === 'documentSearchText')
        {
            const searchText = ReplaceString(e.target.name, '+', '%2B');
            this.updatedData=searchText;
        }
    }

    supplierSearchHandler = (e) => {
        e.preventDefault();
        this.props.actions.FetchSupplierSearchList(this.updatedData).then(res=>{
            this.supplierPanelClickHandler();
            this.child.moveToFirstPage();        
        });
    }

    clearSearchData = () => {
        this.updatedData={};
        this.props.actions.ClearSupplierSearchList();
    }

    render(){
        const { documentTypesData }=this.props;
        const masterDocData=documentTypesData&&documentTypesData.filter(x=>x.moduleName==="Supplier");
        // const defaultSupplierNameSortOrder =[
        //     { "colId": "supplierName",
        //       "sort": "asc" },
        // ];
        
          const supplierSearchDatas=caseInsensitiveSort(this.props.supplierSearchList);

        return(
            <Fragment>
                <div className="companyPageContainer customCard">
                    <Panel 
                        colSize="s12" 
                        isArrow={true}
                        heading={`${ localConstant.supplier.SEARCH_SUPPLIER }`} 
                        subtitle={` : ${ localConstant.supplier.EDIT_VIEW_SUPPLIER }`}
                        onpanelClick={this.supplierPanelClickHandler} 
                        isopen={this.state.isPanelOpen} >
                        <SupplierSearch 
                            country = {this.props.country}
                            placeChangeHandler = {this.placeChangeHandler}
                            state = {this.props.state}
                            city = {this.props.city}
                            onRef = {ref => { this.child = ref; }}
                            searchChangeHandler = {this.formInputChangeHandler}
                            supplierSearch = {this.supplierSearchHandler}
                            clearSearchData = {this.clearSearchData}
                            documentMasterData={masterDocData}
                        />
                    </Panel>
                    <ReactGrid 
                        onRef={ref => {this.child =ref;}}
                        gridRowData={supplierSearchDatas} 
                        gridColData={HeaderData}
                        // columnPrioritySort={defaultSupplierNameSortOrder}
                        />
                </div>
            </Fragment>
        );
    }
}

export default supplierSearch;