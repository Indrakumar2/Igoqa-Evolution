import React, { Component } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './techSpecSearchHeader';
import Panel from '../../../../common/baseComponents/panel';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import arrayUtil from '../../../../utils/arrayUtil';
import { getlocalizeData,formInputChangeHandler, isEmpty } from '../../../../utils/commonUtils';
import { applicationConstants } from '../../../../constants/appConstants';
import { validateObjectHasValue } from '../../../../utils/objectUtil';
import ErrorList from '../../../../common/baseComponents/errorList';
import Modal from '../../../../common/baseComponents/modal';
import { defaultProps } from 'recompose';

const localConstant = getlocalizeData();
export const SearchTechSpec = (props) => (
    <form onSubmit={props.techSpecSearch} onReset={props.clearSearchData} autoComplete="off">
    <div className="row mb-0">
    <div className="col s12"> 
    <CustomInput
            hasLabel={true}
            name="firstName"
            divClassName="col pr-0 pl-0"
            colSize='s2'
            label={localConstant.techSpec.contactInformation.FIRST_NAME}
            type='text'
            inputClass="customInputs"
            maxLength={60}
            onValueChange={props.inputHandleChange}
        />
        <CustomInput
            hasLabel={true}
            name="lastName"
            defaultValue={props.lastName}
            colSize='s2'
            label={localConstant.techSpec.contactInformation.LAST_NAME}
            type='text'
            inputClass="customInputs"
            maxLength={60}
            onValueChange={props.inputHandleChange} />
        <CustomInput
            hasLabel={true}
            name="pin"
            divClassName='pl-0'
            colSize='s2 clearMb'
            label={localConstant.techSpec.techSpecSearch.PIN}
            type='text'
            inputClass="customInputs"
            maxLength={10}
            onValueChange={props.inputHandleChange} />
        <CustomInput
            hasLabel={true}
            name="employmentStatus"
            divClassName='col pl-0'
            colSize='s3'
            label={localConstant.techSpec.techSpecSearch.EMPLOYMENT_STATUS}
            type='select'
            inputClass="customInputs"
            optionsList={props.employmentStatus}
            optionName="name"
            optionValue="name"
            onSelectChange={props.inputHandleChange} />
            <CustomInput
            hasLabel={true}
            name="profileStatus"
            divClassName='col pl-0'
            colSize='s3'
            label={localConstant.techSpec.techSpecSearch.PROFILE_STATUS}
            type='select'
            inputClass="customInputs"
            optionsList={props.profileStatus}
            optionName="name"
            optionValue="name"
            onSelectChange={props.inputHandleChange} />
    </div>
        <div className="col s12">
            <CustomInput
                hasLabel={true}
                name="category"
                divClassName="col pr-0 pl-0"
                colSize={ props.isfromNDTAssignment ? 's4' :'s2' } /** Changes for Hot Fixes on NDT */
                label={localConstant.techSpec.techSpecSearch.CATEGORY}
                type='select'
                inputClass="customInputs"
                optionsList={props.category}
                optionName={ props.isfromNDTAssignment ? "category" : "name"}
                optionValue={ props.isfromNDTAssignment ? "category" : "name"}
                onSelectChange={props.inputHandleChange}
                defaultValue={props.selectedCategory ? props.selectedCategory : ''}
                disabled={props.isCategoryDisable === true}
            />
            <CustomInput
                hasLabel={true}
                name="subCategory"
                colSize={ props.isfromNDTAssignment ? 's4' :'s2' } /** Changes for Hot Fixes on NDT */
                divClassName="col"
                label={localConstant.techSpec.techSpecSearch.SUB_CATEGORY}
                type='select'
                inputClass="customInputs"
                optionsList={props.subCategory}
                optionName="taxonomySubCategoryName"
                optionValue="taxonomySubCategoryName"
                disabled={ props.isSubCategoryDisable ? true : props.subCategory && props.subCategory.length > 0 ? false : true}
                onSelectChange={props.inputHandleChange}
                defaultValue={props.selectedSubCategory ? props.selectedSubCategory : ''}
            />
            <CustomInput
                hasLabel={true}
                name="service"
                divClassName="col pl-0"
                colSize={ props.isfromNDTAssignment ? 's4' :'s2' } /** Changes for Hot Fixes on NDT */
                label={localConstant.techSpec.techSpecSearch.SERVICE}
                type='select'
                inputClass="customInputs"
                optionsList={props.services}
                optionName="taxonomyServiceName"
                optionValue="taxonomyServiceName"
                disabled={props.isServiceDisable ? true : props.services && props.services.length > 0 ? false : true}
                onSelectChange={props.inputHandleChange}
                defaultValue={props.selectedService ? props.selectedService : ''}
            />
            {/** Changes for Hot Fixes on NDT */} 
           { !props.isfromNDTAssignment &&  
           <CustomInput 
                hasLabel={true}
                name="companyCode"
                colSize='s6'
                label={localConstant.techSpec.techSpecSearch.COMPANY_NAME}
                type='text'
                inputClass="customInputs"
                maxLength={60}
                onValueChange={props.inputHandleChange}
                disabled={ true } 
                defaultValue= { props.selectedCompanyName} 
                />
        /** Changes for D549 */
        //    <CustomInput
        //         hasLabel = {true}
        //         name="companyCode"
        //         divClassName="col pl-0"
        //         colSize="s6"
        //         label={localConstant.techSpec.techSpecSearch.COMPANY_NAME}
        //         type='select'
        //         inputClass="customInputs"
        //         onSelectChange = {props.inputHandleChange}
        //         optionsList = {props.companyList}
        //         optionName="companyName"  
        //         optionValue="companyCode"  
        //         disabled={ true } 
        //         defaultValue= { props.selectedCompany}
        //    />
      }
        </div>
        <div className="col s12" >
            <CustomInput
                hasLabel={true}
                name="country"
                colSize='s2'
                divClassName="col pr-0 pl-0"
                label={localConstant.techSpec.techSpecSearch.COUNTRY}
                type='select'
                inputClass="customInputs"
                optionsList={props.countryMasterData}
                optionName="name"
                optionValue="id"
                id="countryId" //Added for ITK D1536
                onSelectChange={props.inputHandleChange} />
            <CustomInput
                hasLabel={true}
                name="county"
                colSize='s2'
                divClassName="col"
                label={localConstant.techSpec.techSpecSearch.STATE_COUNTY_PROVINCE}
                type='select'
                optionsList={props.stateMasterData}
                optionName="name"
                optionValue="id"
                id="countyId" //Added for ITK D1536
                inputClass="customInputs"
                onSelectChange={props.inputHandleChange} 
                disabled={ props.stateMasterData.length > 0 ? false : true}/>
            <CustomInput
                hasLabel={true}
                name="city"
                divClassName="col pl-0"
                colSize='s2'
                label={localConstant.techSpec.contactInformation.CITY}
                type='select'
                inputClass="customInputs"
                optionsList={props.cityMasterData}
                optionName="name"
                optionValue="id"
                id="cityId" //Added for ITK D1536
                onSelectChange={props.inputHandleChange}
                disabled={ props.cityMasterData.length > 0 ? false : true} />
            <CustomInput
                hasLabel={true}
                name="postalCode"
                divClassName="col pl-0"
                colSize='s3'
                label={localConstant.techSpec.techSpecSearch.POST_ZIP_CODE}
                type='text'
                inputClass="customInputs"
                maxLength={10}
                onValueChange={props.inputHandleChange} />
            <CustomInput
                hasLabel={true}
                name="fullAddress"
                divClassName="col pl-0"
                colSize='s3'
                label={localConstant.techSpec.techSpecSearch.FULL_ADDRESS}
                type='text'
                inputClass="customInputs"
                onValueChange={props.inputHandleChange}
                maxLength={250}
            />
            {/* <CustomInput
                hasLabel={true}
                name="technicalDiscipline"
                divClassName='col pl-0'
                colSize='s12'
                label={localConstant.techSpec.techSpecSearch.TECHNICAL_DISCIPLINE}
                type='select'
                inputClass="customInputs"                   
                optionName="name"
                optionValue="name"
                disabled={true}
                onSelectChange={props.inputHandleChange}  /> */}
        </div>
       
        <div className="col s8">
        <CustomInput
                hasLabel={true}
                name="searchDocumentType"  /** Changes for Hot Fixes on NDT */
                colSize='s6'
                divClassName="col pr-0"
                label={localConstant.techSpec.techSpecSearch.SEARCH_DOCUMENTS}
                type='select'                     /** Changes for Hot Fixes on NDT */
                optionsList={props.techSpecDocumentTypeData}
                optionName='name'
                optionValue="name"
                inputClass="customInputs"
                onSelectChange={props.inputHandleChange}
                maxLength={250}
            />
            <CustomInput
                hasLabel={true}
                name="documentSearchText"
                colSize='s6'
                label={localConstant.techSpec.techSpecSearch.SEARCH_DOCUMENTS_FOR_WORDS}
                type='text'
                inputClass="customInputs"
                onValueChange={props.inputHandleChange}
                maxLength={250}
            />
        </div>
        <div className="col s3 pl-0 mt-4x" >
                <button type="submit" className="waves-effect btn ">Search</button>
                <button type="reset" className="ml-2 waves-effect btn" >Reset</button>
        </div>
        
    </div>
             
    </form>
);

class TechSpecSearch extends Component {
    constructor(props) {
        super(props);        
        this.state = {
            isPanelOpen:true,
            errorList: [],
        };
        this.header=this.filterGridColumns();        
        this.updatedData = {};        
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
    //D822 LEVEL 0 AND LEVEL 1 hide columns
    filterGridColumns = () => {
        const tabsToHide = [];
        const removeData = { ...HeaderData };
        const getData = removeData.columnDefs;
        if (this.props.activities[0].activity === "S00001" || this.props.activities[0].activity === "S00003") {                        
            tabsToHide.push(...[
                "Country", "City","State/County/Province","Post/Zip Code","Full Address","Mobile No","Email Address"
              ]);
              const resourceHeaderData = arrayUtil.negateFilter(getData, 'headerName', tabsToHide);                         
              removeData.columnDefs = resourceHeaderData;
              return removeData;
        }
        else
        {
            return HeaderData;
        }
      }
      
    componentWillUnmount(){
        this.props.actions.TechSpecClearSearch();
        this.props.actions.ClearSubCategory();
        this.props.actions.ClearStateCityData();
        this.updatedData={};
    }
    //Panel Click Event Handler
    panelClick = () => {
        this.setState({ isPanelOpen:!this.state.isPanelOpen });      
        //this.props.actions.ShowHidePanel();
    }  
    
    getSateAndCity =(selectedValueName)=>{            
        if (selectedValueName === "country") {
            this.props.actions.ClearStateCityData();
            const selectedCountry = this.updatedData[selectedValueName];
            this.updatedData["county"] = "";
            this.updatedData["city"] = "";
            this.props.actions.FetchStateId(selectedCountry);                     
            
        }
        if (selectedValueName === "county") {
            this.props.actions.ClearCityData(); 
            const selectedState = this.updatedData[selectedValueName];
            this.updatedData["city"] = "";
            this.props.actions.FetchCityId(selectedState);
        }        
    }
    getCategoryAndService=(selectedName)=>{       
        if(selectedName === 'category'){
            this.props.actions.ClearSubCategory();
            const category = this.updatedData[selectedName];
            this.updatedData["subCategory"] = "";
            this.updatedData["service"] = "";
            if(category){
                this.props.actions.FetchTechSpecSubCategory(category);
            }
        }
        if(selectedName === 'subCategory'){
            this.props.actions.ClearServices();
            const subCategory = this.updatedData[selectedName];
            const category = this.updatedData['category'];
            this.updatedData["service"] = "";
            if(subCategory){
                this.props.actions.FetchTechSpecServices(category,subCategory);//def 916 fix
            }
        }
    }
    //All Input Handle get Name and Value
    inputHandleChange =(e)=>{ 
        e.preventDefault();
        const inputvalue = formInputChangeHandler(e);
        this.updatedData[inputvalue.name] = inputvalue.value;

        if(inputvalue.name === 'country' || inputvalue.name === 'county' || inputvalue.name === 'city'){
            this.updatedData[e.target.id] = parseInt(e.target.value);
            this.getSateAndCity(inputvalue.name);
        }
        if(inputvalue.name ==='category'||inputvalue.name ==='subCategory' ||  inputvalue.name === 'service'){
           this.getCategoryAndService(inputvalue.name);
        }        
                   
    }
    //Form Search 
    techSpecFormSearch=(e)=>{ 
        e.preventDefault();  
        if(this.props.selectedCompany)
            this.updatedData["companyCode"] = this.props.selectedCompany;
        const isValid = validateObjectHasValue(this.updatedData, Object.keys(applicationConstants.resourceSearchValidateFields));
        if(!isValid){
            this.setState({
                errorList: Object.values(applicationConstants.resourceSearchValidateFields)
            });
            return false;
        }
        this.setState({ isPanelOpen:!this.state.isPanelOpen }); 
      //  this.props.actions.ClearSubCategory();       
        this.props.actions.TechSpecClearSearch();
        const data = this.updatedData;
        this.props.actions.FetchTechSpecData(this.updatedData);
    }

    closeErrorList = (e) => {
        e.preventDefault();
        this.setState({
            errorList: []
        });
    }

    //Clearing From Search 
    techSpecClearSearchData=()=>{
    this.props.actions.TechSpecClearSearch();
    this.props.actions.ClearSubCategory();
    this.props.actions.ClearStateCityData();
    this.props.actions.SetCurrentPageMode(null);       
    this.updatedData={};
    }
    componentDidMount(){ 
       this.props.actions.grmSearchMatsterData(); 
       this.props.actions.FetchProfilestatus();
       this.props.actions.FetchUserRoleCompanyList(this.props.userName);
    }    
    
    render() {
        const { countryMasterData,stateMasterData,cityMasterData,category,subCategory,services,employmentStatus,profileStatus,userRoleCompanyList,selectedCompany } = this.props;
        /** Added for D549 */
       const selectedCompanyName = !isEmpty(this.props.selectedCompanyName) && this.props.selectedCompanyName.companyName;
        return (
            <div className="customCard">
                <Panel colSize="s12" heading={`${
                        localConstant.techSpec.SEARCH_TECHSPECH
                        }`} subtitle={` : ${ localConstant.techSpec.EDIT_VIEW_TECHSPEC }`} onpanelClick={this.panelClick} isArrow={true} isopen={this.state.isPanelOpen} >
                    <SearchTechSpec 
                     inputHandleChange={(e) => this.inputHandleChange(e)}  
                     countryMasterData={countryMasterData}
                     stateMasterData={stateMasterData}
                     cityMasterData={cityMasterData}
                     category={category}
                     subCategory={subCategory}
                     services={services}
                     employmentStatus={employmentStatus}
                     profileStatus={arrayUtil.sort(profileStatus, 'name', 'asc')}
                     companyList = {userRoleCompanyList}
                     techSpecSearch={this.techSpecFormSearch}
                     clearSearchData={this.techSpecClearSearchData}
                     selectedCompanyName = {selectedCompanyName} //Changes for D549
                     isfromNDTAssignment= {false}
                     techSpecDocumentTypeData={this.props.documentTypeMasterData.filter(x => x.moduleName === localConstant.techSpec.TECH_SPEC)} /> {/** Changes for Hot Fixes on NDT */}
                </Panel>
				<ReactGrid gridRowData={this.props.techSpecSearchData && this.props.techSpecSearchData.filter(x=>x.companyCode===this.props.selectedCompany)} gridColData={this.header}/>
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

export default TechSpecSearch;
