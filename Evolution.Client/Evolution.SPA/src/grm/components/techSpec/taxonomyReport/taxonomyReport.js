import React, { Component } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import Panel from '../../../../common/baseComponents/panel';
import { HeaderData } from './headerData';
import PropTypes from 'prop-types';
import { getlocalizeData,isEmpty,formInputChangeHandler,addElementToArrayParam } from '../../../../utils/commonUtils';
import arrayUtil from '../../../../utils/arrayUtil';
import CustomMultiSelect from '../../../../common/baseComponents/multiSelect';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';

const localConstant = getlocalizeData();
const defaultSearchField={
    companyId:[],
    resource:[],
    epin:[],
    profileStatus:'',
    employmentStatus:'',
    approvalStatus:'',
    category:'',
    subCategory:'',
    service:''
};
const TaxonomyReportFilter = (props) => (
    <form id ="instantSearch"
        onSubmit={props.searchReport}
        onReset={props.clearSearchData} autoComplete="off">
        <div className="row mb-0">
            <div className="col s12">
            <CustomMultiSelect hasLabel={true}
                name="companyCode"
                id="Company"
                divClassName="col"
                label={localConstant.gridHeader.COMPANY_NAME}
                labelClass="mandate"
                type='multiSelect'
                colSize='s4 pl-4'
                optionsList={props.companyList}
                optionLabelName={'companyName'}
                onSelectChange={props.onChangeHandler}
                className="browser-default"
                defaultValue={props.selectedCompanies}
                multiSelectdValue={props.companyChangeHandler}
            /> 
                <CustomMultiSelect hasLabel={true}
                    name="resourceName"
                    id="epin"
                    divClassName="col"
                    label={localConstant.techSpec.timeOffRequest.RESOURCE_NAME}
                    type='multiSelect'
                    colSize='s4'
                    optionsList={props.resourceNameList}
                    optionLabelName={'fullName'}
                    // onSelectChange={props.onChangeHandler}
                    className="browser-default"
                    defaultValue={props.selectedResources}
                    multiSelectdValue={props.resourceChangeHandler}
                    disabled={props.selectedCompanies && props.selectedCompanies.length >0 ?false :true}
                /> 
                 <CustomMultiSelect hasLabel={true}
                    name="epin"
                    id="epin"
                    divClassName="col"
                    label={localConstant.gridHeader.EPIN}
                    type='multiSelect'
                    colSize='s4'
                    optionsList={props.ePinList}
                    optionLabelName={'pin'}
                    // onSelectChange={props.onChangeHandler}
                    className="browser-default"
                    defaultValue={props.selectedEpins}
                    multiSelectdValue={props.ePinChangeHandler}
                    disabled={props.selectedCompanies && props.selectedCompanies.length >0 ?false :true}
                /> 
            </div>
            <div className="col s12">
                <CustomInput
                    hasLabel={true}
                    name="profileStatus"
                    divClassName='col pl-4'
                    colSize='s4'
                    label={localConstant.techSpec.techSpecSearch.PROFILE_STATUS}
                    type='select'
                    inputClass="customInputs"
                    optionsList={props.profileStatus}
                    optionName="name"
                    optionValue="name"
                    defaultValue={props.updatedData.profileStatus ?props.updatedData.profileStatus : ''}
                    onSelectChange={props.inputHandleChange} />
                <CustomInput
                    hasLabel={true}
                    name="employmentStatus"
                    divClassName='col'
                    colSize='s4'
                    label={localConstant.gridHeader.EMPLOYMENT_TYPE}
                    type='select'
                    inputClass="customInputs"
                    optionsList={props.employmentStatus}
                    optionName="name"
                    optionValue="name"
                    defaultValue={props.updatedData.employmentStatus ?props.updatedData.employmentStatus : ''}
                    onSelectChange={props.inputHandleChange} />
                <CustomInput
                    hasLabel={true}
                    name="approvalStatus"
                    divClassName='col'
                    colSize='s4'
                    label={localConstant.gridHeader.APPROVAL_STATUS}
                    type='select'
                    inputClass="customInputs"
                    optionsList={localConstant.commonConstants.approvalStatus}
                    optionName="label"
                    optionValue="value"
                    defaultValue={props.updatedData.approvalStatus ?props.updatedData.approvalStatus : ''}
                    onSelectChange={props.inputHandleChange} />
            </div>
            <div className="col s12">
                <CustomInput
                    hasLabel={true}
                    name="category"
                    divClassName="col pl-4"
                    colSize='s4'
                    label={localConstant.techSpec.techSpecSearch.CATEGORY}
                    type='select'
                    inputClass="customInputs"
                    optionsList={props.category}
                    optionName="name"
                    optionValue="name"
                    onSelectChange={props.inputHandleChange}
                    defaultValue={props.updatedData.category ?props.updatedData.category : ''}
                    disabled={props.isCategoryDisable === true}
                />
                <CustomInput
                    hasLabel={true}
                    name="subCategory"
                    colSize='s4'
                    divClassName="col"
                    label={localConstant.techSpec.techSpecSearch.SUB_CATEGORY}
                    type='select'
                    inputClass="customInputs"
                    optionsList={props.subCategory}
                    optionName="taxonomySubCategoryName"
                    optionValue="taxonomySubCategoryName"
                    disabled={ props.subCategory && props.subCategory.length > 0 ? false : true}
                    onSelectChange={props.inputHandleChange}
                    defaultValue={props.updatedData.subCategory ?props.updatedData.subCategory : ''}
                />
                <CustomInput
                    hasLabel={true}
                    name="service"
                    divClassName="col "
                    colSize='s4'
                    label={localConstant.techSpec.techSpecSearch.SERVICE}
                    type='select'
                    inputClass="customInputs"
                    optionsList={props.services}
                    optionName="taxonomyServiceName"
                    optionValue="taxonomyServiceName"
                    disabled={ props.services && props.services.length > 0 ? false : true}
                    onSelectChange={props.inputHandleChange}
                    defaultValue={props.updatedData.service ?props.updatedData.service : ''}
                />
            </div>
        </div>
        <div className="col pl-4 mr-2 right" >
            <button type="submit" className="waves-effect btn ">Search</button>
            <button type="reset" className="ml-2 waves-effect btn" >Reset</button>
        </div>
    </form>
);
class TaxonomyReport extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isPanelOpen: true,
            selectedCompanies:[],
            selectedResources:[],
            selectedEpins:[],
        };
        this.updatedData = defaultSearchField;
    }
    /**panel click action */
    panelClick = (e) => {
        this.setState((state) => {
            return {
                isPanelOpen: !state.isPanelOpen
            };
        });
    }
    /**On Company Change  */
    componentDidMount() {
        //this.props.actions.FetchTimeOffRequestData();
        this.props.actions.grmSearchMatsterData(); 
        this.props.actions.FetchProfilestatus();
        this.props.actions.ClearSubCategory();
        this.props.actions.ClearServices();
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
    inputHandleChange =(e)=>{ 
        e.preventDefault();
        const inputvalue = formInputChangeHandler(e);
        this.updatedData[inputvalue.name] = inputvalue.value;
        if(inputvalue.name ==='category'||inputvalue.name ==='subCategory' ||  inputvalue.name === 'service'){
           this.getCategoryAndService(inputvalue.name);
        }        
    }
    companyChangeHandler =(e)=>{
        this.setState({ selectedCompanies:e });
        this.updatedData["companyIds"]=[];
        const isActive = false;
        if (!isEmpty(e)) {
            e.forEach(x => {
                this.updatedData["companyIds"].push(x.id);
            });
        }
        this.props.actions.FetchResourceInfo(this.updatedData["companyIds"], isActive);
    }
    resourceChangeHandler =(e)=>{
        this.setState({ selectedResources:e });
    }
    ePinChangeHandler =(e)=>{
        this.setState({ selectedEpins:e });
    }
    searchReport =(e)=>{
        e.preventDefault();
        if (isEmpty(this.state.selectedCompanies)) {
            IntertekToaster(localConstant.techSpec.common.REPORT_COMPANY_VALIDATION, 'warningToast company');
            return false;
        }
        this.updatedData["resourceEpins"]=[];
        if (!isEmpty(this.state.selectedResources)) {
            this.state.selectedResources.forEach(x => {
                this.updatedData["resourceEpins"].push(x.pin);
            });
        }
        this.updatedData["epinList"]=[];
        if (!isEmpty(this.state.selectedEpins)) {
            this.state.selectedEpins.forEach(x => {
                this.updatedData["epinList"].push(x.pin);
            });
        }
        this.props.actions.FetchTaxonomyReport(this.updatedData);
    }
    clearSearchData = ()=>{
        this.setState({ selectedCompanies: [], selectedResources:[], selectedEpins: [] });
        this.updatedData =defaultSearchField;
        this.props.actions.ClearSearchData();
        this.props.actions.ClearSubCategory();
        this.props.actions.ClearServices();
    }
    render() {
        const { category,subCategory,services,employmentStatus,profileStatus,userRoleCompanyList,selectedCompany } = this.props;
        return (
            <div className="companyPageContainer customCard">
                <Panel colSize="s12"
                    isArrow={true}
                    heading={`${
                        localConstant.techSpec.common.TAXONOMY_REPORT
                        }`}
                    onpanelClick={this.panelClick}
                    isopen={this.state.isPanelOpen} >

                    <TaxonomyReportFilter
                        companyList={!isEmpty(this.props.companyList) ? addElementToArrayParam(this.props.companyList,'companyName','companyName') :[] }
                        inputHandleChange={(e) => this.inputHandleChange(e)}
                        resourceNameList={!isEmpty(this.props.companyBasedTSData) ? addElementToArrayParam(arrayUtil.sort(this.props.companyBasedTSData,'fullName','asc'), 'fullName','fullName'):[]}
                        ePinList={!isEmpty(this.props.companyBasedTSData) ? addElementToArrayParam(this.props.companyBasedTSData,'pin','pin'):[]}
                        category={category}
                        subCategory={subCategory}
                        services={services}
                        employmentStatus={employmentStatus}
                        profileStatus={arrayUtil.sort(profileStatus, 'name', 'asc')}
                        companyChangeHandler={(e)=>this.companyChangeHandler(e)}
                        resourceChangeHandler={(e)=>this.resourceChangeHandler(e)}
                        ePinChangeHandler={(e)=>this.ePinChangeHandler(e)}
                        searchReport={(e) => this.searchReport(e)}
                        updatedData ={this.updatedData}
                        clearSearchData ={this.clearSearchData}
                        selectedCompanies={this.state.selectedCompanies}
                        selectedResources={this.state.selectedResources}
                        selectedEpins={this.state.selectedEpins}
                    />

                </Panel>

                <div className="customCard">
                    <ReactGrid
                        gridCustomClass={'customPinnedGrid reactAgGrid'}
                        gridRowData={this.props.taxonomyReportData}
                        gridColData={HeaderData} />
                </div>
            </div>

        );
    }
}

export default TaxonomyReport;

TaxonomyReport.propTypes = {
    companyList: PropTypes.array

};

TaxonomyReport.defaultProps = {
    companyList: []
};