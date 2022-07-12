import React, { Component } from 'react';
import { getlocalizeData,formInputChangeHandler,isEmptyOrUndefine } from '../../utils/commonUtils';
import { SearchTechSpec } from '../../grm/components/techSpec/techSpecSearch/techSpecSearch';
import Panel from '../../common/baseComponents/panel';
import ReactGrid from '../../common/baseComponents/reactAgGrid';

const localConstant = getlocalizeData();

class TechSpecSearch extends Component {
    constructor(props){
        super(props);
        this.state={
            isPanelOpen:true,
            companyCode: '',
        };
        this.updatedData={};
    }

    componentDidMount() {
        this.props.actions.grmSearchMatsterData();
        this.props.actions.FetchProfilestatus();
        const { selectedService,
            selectedCategory,
            selectedSubCategory,
            ocCompanyCode } = this.props; //Changes for Hot Fixes on NDT
        if (selectedCategory) {
            this.updatedData.category = selectedCategory;
        }
        if (selectedSubCategory) {
            this.updatedData.subCategory = selectedSubCategory;
        }
        if (selectedService) {
            this.updatedData.service = selectedService;
        }
        if (ocCompanyCode) { //Changes for Hot Fixes on NDT
            this.updatedData.companyCode = ocCompanyCode;
            this.setState({ companyCode: ocCompanyCode });
        }
        if (this.props.isfromNDTAssignment) { //Changes for Hot Fixes on NDT
            this.props.actions.TechSpecClearSearch();
            this.props.actions.FetchTechSpecData(this.updatedData, this.props.isfromNDTAssignment);
        }
        if (this.props.selectedLastName) {
            if (this.props.selectedCompany) {
                this.updatedData["companyCode"] = this.props.selectedCompany;
            }
            this.updatedData["lastName"] = this.props.selectedLastName;
            this.props.actions.TechSpecClearSearch();
            this.props.actions.FetchTechSpecData(this.updatedData, false);
        }
    }

    componentWillUnmount(){
        this.props.actions.ClearStateCityData();
        this.props.actions.clearNDTSubCategory();
        this.updatedData={};
    }

    panelClick = () => {
        this.setState({ isPanelOpen:!this.state.isPanelOpen });
    }

    getSateAndCity =(selectedValueName)=>{            
        if (selectedValueName === "country") {
            this.props.actions.ClearStateCityData();
            const selectedCountry = this.updatedData[selectedValueName];
            this.updatedData['county'] = '';
            this.updatedData['city'] = '';
            this.props.actions.FetchStateId(selectedCountry);                     
            
        }
        if (selectedValueName === "county") {
            this.props.actions.ClearCityData(); 
            const selectedState = this.updatedData[selectedValueName];
            this.updatedData['city'] = '';
            this.props.actions.FetchCityId(selectedState);
        }        
    }
    getCategoryAndService=(selectedName)=>{       
        if(selectedName === 'category'){
            this.props.actions.clearNDTSubCategory();
            const category = this.updatedData[selectedName];
            this.updatedData['subCategory'] = '';
            this.updatedData['service'] = '';
            this.props.actions.FetchNDTTaxonomySubCategory(category);
        }
        if(selectedName === 'subCategory'){
            this.props.actions.FetchNDTTaxonomyService();
            const subCategory = this.updatedData[selectedName];
            this.updatedData['service'] = '';
            this.props.actions.FetchNDTTaxonomyService(subCategory);
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
        this.setState({ isPanelOpen:!this.state.isPanelOpen }); 
        // this.props.actions.ClearSubCategory();       
        this.props.actions.TechSpecClearSearch();
        if (this.state.companyCode && this.state.companyCode != '') {
            this.updatedData.companyCode = this.state.companyCode;
        } else {
            this.updatedData.companyCode = this.props.selectedCompany;
        }
        this.props.actions.FetchTechSpecData(this.updatedData, this.props.isfromNDTAssignment);        
    }

    //Clearing From Search 
    techSpecClearSearchData = () => {
        this.props.actions.TechSpecClearSearch();
        // this.props.actions.ClearSubCategory();
        this.updatedData = {}; //Changes for Hot Fixes on NDT
    }

    render(){
        const { category,
            services,
            subCategory,
            countryMasterData,
            stateMasterData,
            cityMasterData,
            employmentStatus,
            selectedService,
            selectedCategory,
            profileStatus,
            selectedSubCategory,
            selectedLastName,
            assignedTSList,
            techSpecSearchData } = this.props; //Changes for Hot Fixes on NDT
            const searchData=techSpecSearchData && techSpecSearchData.filter(search => { return assignedTSList && !assignedTSList.some(assigned => { return assigned.epin === search.epin; }); });
         return(
            <div className="customCard">
                <Panel heading="Resource" subtitle=": Search" colSize="s12" onpanelClick={this.panelClick} isopen={this.state.isPanelOpen} isArrow={true}>
                    <SearchTechSpec
                        services={ services}
                        selectedService={selectedService}
                        isServiceDisable={false}
                        category={category}
                        selectedCategory={selectedCategory}
                        isCategoryDisable={false}
                        subCategory={subCategory}
                        lastName={selectedLastName}
                        selectedCompany={this.props.selectedCompany}
                        selectedCompanyName={this.props.selectedCompanyName}
                        selectedSubCategory={selectedSubCategory}
                        isSubCategoryDisable={false}
                        techSpecSearch={this.techSpecFormSearch}
                        clearSearchData={this.techSpecClearSearchData}
                        inputHandleChange={this.inputHandleChange}
                        countryMasterData={countryMasterData}
                        stateMasterData={stateMasterData}
                        cityMasterData={cityMasterData}
                        employmentStatus={employmentStatus} 
                        profileStatus={profileStatus}
                        isfromNDTAssignment={this.props.isfromNDTAssignment} 
                        techSpecDocumentTypeData={this.props.documentTypeMasterData.filter(x => x.moduleName === localConstant.techSpec.TECH_SPEC)} /> {/** Changes for Hot Fixes on NDT */}
                </Panel>
                <ReactGrid gridRowData={!isEmptyOrUndefine(assignedTSList)?searchData:techSpecSearchData} gridColData={this.props.headerData} onRef={this.props.gridRef} /> {/** Changes for Hot Fixes on NDT */}
            </div>
        );
    }
};

export default TechSpecSearch;