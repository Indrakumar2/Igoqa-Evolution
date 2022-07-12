import React, { Component, Fragment } from 'react';
import arrayUtil from '../../../../utils/arrayUtil';
import {
    getlocalizeData,
    GenerateReport,
    ObjectIntoQuerySting,
    isEmpty
} from '../../../../utils/commonUtils';
import Modal from '../../../../common/baseComponents/modal';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import PropTypes from 'prop-types';
import { EvolutionReportsAPIConfig  } from '../../../../apiConfig/apiConfig';
import { DownloadReportFile }  from '../../../../common/reportUtil/ssrsUtil';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';

const localConstant = getlocalizeData();

class UnApprovedVisitTimesheetModal extends Component {

    constructor(props) {
        super(props);
        this.state = {
        };
        this.reportParam = {};
        this.reportParam['format'] = localConstant.commonConstants.EXCEL;
        this.excelDatas = {
        };
    }

    componentDidMount() {
        this.loadCustomer();
        let _isOperating = true;
        let _isVisit = false;
        if (this.props.title === localConstant.visit.UNAPPROVED_VISITS_REPORT_CHC || this.props.title === localConstant.visit.UNAPPROVED_VISITS_REPORT_OC) {
            _isVisit = true;
        }
        if (this.props.title === localConstant.visit.UNAPPROVED_VISITS_REPORT_CHC || this.props.title === localConstant.timesheet.UNAPPROVED_TIMESHEET_REPORT_CHC){
            _isOperating = false;
        }
        const Params = {
            "CompanyCode": this.props.selectedCompany,
            "IsVisit":_isVisit,
            "IsOperating":_isOperating
        };
        this.props.actions.FetchReportCoordinator(Params);
    }

    loadCustomer(){
        let _isOperating = true;
        let _isVisit = false;
        if (this.props.title === localConstant.visit.UNAPPROVED_VISITS_REPORT_CHC || this.props.title === localConstant.visit.UNAPPROVED_VISITS_REPORT_OC) {
            _isVisit = true;
        }
        if (this.props.title === localConstant.visit.UNAPPROVED_VISITS_REPORT_CHC || this.props.title === localConstant.timesheet.UNAPPROVED_TIMESHEET_REPORT_CHC){
            _isOperating = false;
        }
        const companyId = Array.isArray(this.props.companyList) && this.props.companyList.filter(x => x.companyCode === this.props.selectedCompany);
        const loggedInCompanyId = companyId[0].companyId;
        const params = {
            "ContractHolderCompanyId":loggedInCompanyId,
            "CoordinatorId": null,
            "isVisit": _isVisit,
            "isOperating" : _isOperating
        };
        this.props.actions.ClearCustomerList();
        this.props.actions.FetchUnApprovedVistCustomer(params);
    }

    componentWillUnmount() {
        this.props.actions.ClearCustomerList();
    }

    exportVisitTimesheetUnapprovedReportHandler = async () => {
        const _localStorage = localStorage.getItem('Username');
        this.reportParam['username'] = _localStorage;
        let reportFileName = '';
        let ContractHolderCompany = false;
        const selectedCoordinatorId = this.reportParam['CoordinatorId'];
        const selectedCustomerId = this.reportParam['CustomerId'];
        const selectedContractId = this.reportParam['ContractId'];
        const selectedProjectId = this.reportParam['ProjectId'];

        if (selectedCoordinatorId == undefined && selectedCustomerId == undefined && selectedContractId == undefined && selectedProjectId == undefined) {
            IntertekToaster(localConstant.commonConstants.ERR_REPORT, 'warningToast ssrs_report_message');
        }
        else {
            const companyId = Array.isArray(this.props.companyList) && this.props.companyList.filter(x => x.companyCode === this.props.selectedCompany);
            const loggedInCompanyId = companyId[0].companyId;
            if (this.props.title === localConstant.visit.UNAPPROVED_VISITS_REPORT_CHC || this.props.title === localConstant.visit.UNAPPROVED_VISITS_REPORT_OC) {
                this.reportParam["reportname"] = EvolutionReportsAPIConfig.UnapprovedVisitsReport;
                reportFileName = 'UnapprovedVisitsReport';
                this.reportParam['reptype'] = 2;
            }
            else if (this.props.title === localConstant.timesheet.UNAPPROVED_TIMESHEET_REPORT_CHC || this.props.title === localConstant.timesheet.UNAPPROVED_TIMESHEET_REPORT_OC) {
                this.reportParam["reportname"] = EvolutionReportsAPIConfig.UnapprovedTimesheetReport;
                reportFileName = 'UnapprovedTimesheetReport';
                this.reportParam['reptype'] = 3;
            }
            if (this.props.title === localConstant.timesheet.UNAPPROVED_TIMESHEET_REPORT_CHC || this.props.title === localConstant.visit.UNAPPROVED_VISITS_REPORT_CHC) {
                ContractHolderCompany = true;
                this.reportParam['ContractHolderCompanyId'] = loggedInCompanyId;
                this.reportParam['OperatingCompanyId'] = undefined;
            }
            else if (this.props.title === localConstant.visit.UNAPPROVED_VISITS_REPORT_OC || this.props.title === localConstant.timesheet.UNAPPROVED_TIMESHEET_REPORT_OC) {
                this.reportParam['OperatingCompanyId'] = loggedInCompanyId;
                this.reportParam['ContractHolderCompanyId'] = undefined;
            }
            this.reportParam['background'] = true;
            this.reportParam["ContractHolderCompany"] = ContractHolderCompany;
            DownloadReportFile(this.reportParam, 'application/vnd.ms-excel', reportFileName, '.xls', this.props);
        }
    }

    handlerChange = (e) => {  
        if(e.target.value != undefined && e.target.value != ''){
            let _isOperating = false;
            let _isVisit = false;
            const companyId = Array.isArray(this.props.companyList) && this.props.companyList.filter(x => x.companyCode === this.props.selectedCompany);
            const loggedInCompanyId = companyId[0].companyId;
            if (this.props.title === localConstant.visit.UNAPPROVED_VISITS_REPORT_CHC || this.props.title === localConstant.visit.UNAPPROVED_VISITS_REPORT_OC) {
                _isVisit = true;
            }
            if (this.props.title === localConstant.visit.UNAPPROVED_VISITS_REPORT_CHC || this.props.title === localConstant.timesheet.UNAPPROVED_TIMESHEET_REPORT_CHC) {
                this.reportParam['ContractHolderCompanyId'] = loggedInCompanyId;
                this.reportParam['OperatingCompanyId'] = undefined;
                _isOperating = false;
            }
            else if (this.props.title === localConstant.visit.UNAPPROVED_VISITS_REPORT_OC || this.props.title === localConstant.timesheet.UNAPPROVED_TIMESHEET_REPORT_OC) {
                this.reportParam['OperatingCompanyId'] = loggedInCompanyId;
                this.reportParam['ContractHolderCompanyId'] = undefined;
                _isOperating = true;
            }
            if (e.target.name === "coordinators") {
                this.reportParam['CoordinatorId'] = e.target.value;
                const params = {
                    "ContractHolderCompanyId":loggedInCompanyId,
                    "CoordinatorId": e.target.value,
                    "isVisit": _isVisit,
                    "isOperating" : _isOperating
                };
                this.reportParam["CustomerId"] = undefined;
                this.reportParam["ContractId"] = undefined;
                this.reportParam["ProjectId"] = undefined;
                this.props.actions.ClearCustomerList();
                this.props.actions.FetchUnApprovedVistCustomer(params);
            }
            else if (e.target.name === "customers") {
                this.reportParam["CustomerId"] = e.target.options[e.target.selectedIndex].getAttribute('customId');
                this.props.actions.ClearCustomerContracts();
                const data = {
                    "customerCode": e.target.value,
                    "CoordinatorId": this.reportParam['CoordinatorId'],
                    "ContractHolderCompanyId":loggedInCompanyId,
                    "isVisit": _isVisit,
                    "isOperating" : _isOperating
                };
                this.reportParam["ContractId"] = undefined;
                this.reportParam["ProjectId"] = undefined;
                this.props.actions.FetchUnApprovedVistContract(data);
            }
            else if (e.target.name === "contracts") {
                this.reportParam["ContractId"] = e.target.options[e.target.selectedIndex].getAttribute('customId');
                this.props.actions.ClearContractProjects();
                const Params = {
                    "contractNumber" : e.target.value,
                    "ContractHolderCompanyId":loggedInCompanyId,
                    "isApproved" : false,
                    "isVisit" : _isVisit,
                    "isOperating": _isOperating,
                    "isNDT":false,
                    "CoordinatorId": this.reportParam['CoordinatorId']
                };
                this.reportParam["ProjectId"] = undefined;
                this.props.actions.FetchContractProjects(Params);
            }
            else if (e.target.name === "projects") {
                this.reportParam["ProjectId"] = e.target.value;
            }
        }
        else{
            if (e.target.name === "coordinators") {
                this.reportParam['CoordinatorId'] = undefined;
                this.reportParam["CustomerId"] = undefined;
                this.reportParam["ContractId"] = undefined;
                this.reportParam["ProjectId"] = undefined;
                this.props.actions.ClearCustomerContracts();
                this.props.actions.ClearContractProjects();
                this.loadCustomer();
            }
            else if(e.target.name==="customers") {
                this.reportParam["CustomerId"] = undefined;
                this.reportParam["ContractId"] = undefined;
                this.reportParam["ProjectId"] = undefined;
                this.props.actions.ClearCustomerContracts();
                this.props.actions.ClearContractProjects();
            }
            else if (e.target.name === "contracts") {
                this.reportParam["ContractId"] = undefined;
                this.props.actions.ClearContractProjects();
            }
            else if(e.target.name === "projects") {
                this.reportParam["ProjectId"] = undefined;
            }
        }
        
    }

    mergeContracts = (contracts) => {
        const mergedData= Array.isArray(contracts) && contracts.map(items=>
            {
            items['mergedContractData']=`${ items.contractNumber } ${ "|" } ${ items.customerContractNumber }`;
            return items;
            });
         return mergedData;
    }

    mergedProjects = (projects) => { 
        const mergedData= Array.isArray(projects) && projects.map(items=>
            {
            items['mergedProjectData']=`${ items.projectNumber } ${ "|" } ${ items.customerProjectNumber }`;
            return items;
            });
         return mergedData;
    }

    render() {
        const { customerList, contractList, projectList } = this.props;
        const customers = arrayUtil.removeDuplicates(customerList,'customerCode');
        const projectValue = this.mergedProjects(projectList);
    
        return (
     
            <Fragment>
                <Modal id="reportId"
                    title={this.props.title}
                    titleClassName='bold'
                    buttons={[
                        {
                            name: 'Cancel',
                            action: this.props.hidemodal,
                            type: "button",
                            btnClass: 'btn-small modal-close mr-2',
                            showbtn: true
                        },
                        {
                            name: 'View',
                            type: "button",
                            action: this.exportVisitTimesheetUnapprovedReportHandler,
                            btnClass: 'btn-small modal-close',
                            showbtn: true
                        }
                    ]}
                    isShowModal={this.props.showmodal}>
                   <form>
                    <div className="row">
                        <CustomInput
                            hasLabel={true}
                            labelClass="customLabel"
                            name='coordinators'
                            divClassName='col'
                            label={localConstant.contract.COORDINATOR}
                            type='select'
                            onSelectChange={ this.handlerChange }
                            colSize='s12 m6'
                            inputClass="customInputs"
                            required={true}
                            optionName='displayName'
                            optionValue='id'
                            optionsList={ this.props.coordinatorList }
                            autocomplete="off"
                        />
                        <CustomInput
                            hasLabel={true}
                            labelClass="customLabel"
                            name='customers'
                            divClassName='col'
                            label={localConstant.customer.CUSTOMER}
                            type='select'
                            onSelectChange={this.handlerChange}
                            colSize='s12 m6'
                            inputClass="customInputs"
                            required={true}
                            customId='customerId'
                            optionName='customerName'
                            optionValue="customerCode"
                            optionsList={ customers }
                            autocomplete="off"
                        />
                    </div>
                    <div className="row">

                        <CustomInput
                            hasLabel={true}
                            labelClass="customLabel"
                            name='contracts'
                            divClassName='col'
                            label={localConstant.contract.CONTRACTS}
                            type='select'
                            onSelectChange={this.handlerChange}
                            colSize='s12 m6'
                            inputClass="customInputs"
                            required={true}
                            customId='id'
                            optionName='mergedContractData'
                            optionValue="contractNumber"
                            optionsList={ contractList }
                            autocomplete="off"
                        />
                        <CustomInput
                            hasLabel={true}
                            labelClass="customLabel"
                            name='projects'
                            divClassName='col'
                            label={localConstant.project.PROJECTS}
                            type='select'
                            onSelectChange={this.handlerChange}
                            colSize='s12 m6'
                            inputClass="customInputs"
                            required={true}
                            optionName='mergedProjectData'
                            optionValue="projectNumber"
                            optionsList={ projectValue }
                            autocomplete="off"
                        />
                    </div>
                    <div class='row'>
                        <label class='bold'>{localConstant.supplier.FORMAT}</label>
                        <label>
                            <input className="with-gap" defaultChecked={true}
                                value='excel' name="Format" type="radio" />
                            <span>{localConstant.commonConstants.EXCEL}</span>
                        </label>
                    </div>
                    </form>
                </Modal>
            </Fragment>
        );
    }
}

export default UnApprovedVisitTimesheetModal;

UnApprovedVisitTimesheetModal.propTypes = {
    contractList: PropTypes.array,
    projectList: PropTypes.array,
    customerList: PropTypes.array,
};

UnApprovedVisitTimesheetModal.defaultProps = {
    contractList: [],
    projectList:[],
    customerList: [],
};