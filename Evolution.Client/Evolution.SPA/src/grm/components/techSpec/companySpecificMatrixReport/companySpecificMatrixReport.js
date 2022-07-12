import React, { Component } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import Panel from '../../../../common/baseComponents/panel';
import { HeaderData,TaxonomyHeaderData } from './headerData';
import PropTypes from 'prop-types';
import { getlocalizeData,isEmpty, isEmptyReturnDefault } from '../../../../utils/commonUtils';
import CustomMultiSelect from '../../../../common/baseComponents/multiSelect';
import {
    addElementToArrayParam
} from '../../../../utils/commonUtils';
import { DownloadReportFile }  from '../../../../common/reportUtil/ssrsUtil';
import { GrmAPIConfig } from '../../../../apiConfig/apiConfig';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';

const localConstant = getlocalizeData();
const CompanySpecificMatrixReportFilter = (props) => (
    <form id ="companyMatrixSearch"
        onSubmit={props.submitHandler}
        onReset={props.clearSearchData} autoComplete="off">
        <div className="row mb-0">
              <CustomMultiSelect hasLabel={true}
                name="companyCode"
                id="Company"
                divClassName="col"
                label={localConstant.gridHeader.COMPANY_NAME}
                type='multiSelect'
                colSize='s8 pl-4'
                labelClass="customLabel mandate"
                inputClass="customInputs"
                optionsList={props.companyList}
                optionLabelName={'companyName'}
                onSelectChange={props.onChangeHandler}
                className="browser-default"
                defaultValue={props.selectedCompanies}
                multiSelectdValue={props.onCompanyChange}
                allowSelectAll={true}
            /> 
            <button type="submit" className="mt-4x mr-2 modal-close waves-effect waves-green btn " >Search</button>
            <button type="reset" className="mt-4x modal-close waves-effect waves-green btn" >Reset</button>
            </div>
        
    </form>
);
class CompanySpecificMatrixReport extends Component {
    constructor(props) {

        super(props);
        this.state = {
            isPanelOpen: true,
            isResourceBased:false,
            selectedCompanies:[]
        };
        this.groupingParam = 
        { 
          groupName:"resourceName",
          dataName:"childArray" 
        };
        this.searchData = {};
    }
    // componentDidMount(){
    //     this.props.actions.FetchCompanySpecificMatrixDetails();
    // }
    /**panel click action */
    panelClick = (e) => {
        this.setState((state) => {
            return {
                isPanelOpen: !state.isPanelOpen
            };
        });
    }
    /**On Company Change  */
    onCompanyChange = (e) => {
        this.setState({
            selectedCompanies: e
        });
        const companyCodes = [];
        if (!isEmpty(e)) {
            e.forEach(x => {
                companyCodes.push(x.companyCode);
            });
        }
        this.searchData = {
            companyCode: companyCodes.join(',')
        };
    }
    loadResourceBasedData =(e) => {
        if(e.target.name === 'showDataBasedOnResource'){
            this.setState( { isResourceBased:true } );
            if(this.props.resourceTaxonomiesData.length>0)
                this.props.actions.FetchCompanySpecificMatrixDetails(true,this.state.selectedCompanies);
        }
        else if(e.target.name === 'showDataBasedOnTaxonomy'){
            this.setState( { isResourceBased:false } );
            if(this.props.resourceTaxonomiesData.length>0)
                this.props.actions.FetchCompanySpecificMatrixDetails(false,this.state.selectedCompanies);
        }
    }
    submitHandler =(e)=>{
        e.preventDefault();
        if(isEmpty(this.state.selectedCompanies)){
            IntertekToaster(localConstant.techSpec.common.REPORT_COMPANY_VALIDATION, 'warningToast companyNotSelected');
            return false;
        }
        this.props.actions.FetchCompanySpecificMatrixDetails(this.state.isResourceBased,this.state.selectedCompanies);
    }
    clearSearchData = (e)=>{
        this.setState({
            selectedCompanies: []
        });
        this.props.actions.ClearSearchData();
    }
    exportExcel = async (e) => {
        this.props.actions.ShowLoader();
        this.searchData['reportname'] = 'CompanySpecificMatrixReport';
        const URL=GrmAPIConfig.companySpecificMatrixReportExport;
        DownloadReportFile(this.searchData,'application/vnd.ms-excel','CompanySpecificMatrixReport','.csv',this.props,URL);
    }
    render() {
        const rowData =this.props.resourceTaxonomiesData;
        const headerData = this.state.isResourceBased ? HeaderData :TaxonomyHeaderData;  
        return (
            <div className="companyPageContainer customCard">
                <Panel colSize="s12"
                    isArrow={true}
                    heading={`${
                        localConstant.techSpec.common.COMPANY_SPECIFIC_MATRIX_REPORT
                        }`}
                    onpanelClick={this.panelClick}
                    isopen={this.state.isPanelOpen} >

                    <CompanySpecificMatrixReportFilter
                        companyList={!isEmpty(this.props.companyList) ? addElementToArrayParam(this.props.companyList,'companyName','companyName') :[] }
                        onCompanyChange={(e) => this.onCompanyChange(e)}
                        selectedCompanies ={this.state.selectedCompanies}
                        submitHandler={(e)=> this.submitHandler(e)}
                        clearSearchData ={(e)=> this.clearSearchData(e)}
                    />

                </Panel>
                <div className="customCard">
                    <ReactGrid
                        gridRowData={rowData} 
                        gridColData={headerData}
                        isGrouping={true} 
                        expanded={false}
                        groupName={this.groupingParam && this.groupingParam.groupName} 
                        dataName={this.groupingParam && this.groupingParam.dataName}    
                        paginationPrefixId={localConstant.paginationPrefixIds.resourceTaxonomyGrid}
                        exportCSV={this.exportExcel}
                        />
                </div>
                <div>
                    <label>
                        <input className="with-gap" name="showDataBasedOnTaxonomy" value="showDataBasedOnTaxonomy" type="radio" checked={!this.state.isResourceBased} onChange={this.loadResourceBasedData} />
                        <span>Show Data Based on Taxonomy Service</span>
                    </label>
                    <label className='pr-3'>
                        <input className="with-gap" name="showDataBasedOnResource" value="showDataBasedOnResource" type="radio" checked={this.state.isResourceBased} onChange={this.loadResourceBasedData} />
                        <span>Show Data Based on Resource</span>
                    </label>
                </div>
            </div>

        );
    }
}

export default CompanySpecificMatrixReport;

CompanySpecificMatrixReport.propTypes = {
    companyList: PropTypes.array

};

CompanySpecificMatrixReport.defaultProps = {
    companyList: []
};