import React, { Component, Fragment } from 'react';
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

class ApprovedVisitModal extends Component {

    constructor(props) {
        super(props);
        this.state = {
           showTsModal:false,
           isPanelOpen:true,
        };
        this.reportParam = {};
        this.reportParam['format'] = localConstant.commonConstants.EXCEL;
        this.excelDatas = {
        };

    }

    componentDidMount() {
        this.props.actions.ClearCustomerContracts();
        const _username = localStorage.getItem('Username');
        const companyId = Array.isArray(this.props.companyList) && this.props.companyList.filter( x=>x.companyCode===this.props.selectedCompany );
        const loggedInCompanyId = companyId[0].companyId;
        let _isVisit = false;
        let _isNDT = true;
        if(this.props.title===localConstant.visit.APPROVED_VISITS_REPORT_PROFORMA || this.props.title===localConstant.visit.APPROVED_VISITS_PROFORMA_NDT){
          _isVisit = true;
        }
        if(this.props.title===localConstant.visit.APPROVED_VISITS_REPORT_PROFORMA || this.props.title===localConstant.timesheet.APPROVED_TIMESHEET_REPORT_PROFORMA){
            _isNDT = false;
        }
        const data = {
            "ContractHolderCompanyId":loggedInCompanyId,
            "isVisit":_isVisit,
            "isNDT":_isNDT
        };
        this.props.actions.FetchApprovedUnApprovedVistCustomer(data);
    }

    componentWillUnmount() {
        this.props.actions.ClearCustomerContracts();
    }
   
    exportVisitApprovedReportHandler = async () => {
        const _localStorage = localStorage.getItem('Username');
        this.reportParam['username'] = _localStorage;
        let reportFileName = '';
        const CustomerId = this.reportParam["CustomerId"];
        const ContractId = this.reportParam["ContractId"];
        const ProjectId = this.reportParam["ProjectId"];
        const companyId = Array.isArray(this.props.companyList) && this.props.companyList.filter( x=>x.companyCode===this.props.selectedCompany );
        const loggedInCompanyId = companyId[0].companyId;
        this.reportParam["CompanyId"] = loggedInCompanyId;

        if(CustomerId == undefined && ContractId == undefined && ProjectId == undefined){
            IntertekToaster(localConstant.commonConstants.ERR_REPORT, 'warningToast ssrs_report_message');
        }
        else{
            if (this.props.title === localConstant.visit.APPROVED_VISITS_REPORT_PROFORMA) {
                this.reportParam['reportname'] = EvolutionReportsAPIConfig.ApprovedVisitsReport;
                reportFileName = 'ApprovedVisitsReport';
                this.reportParam['isNDT'] = 'false';
                this.reportParam['reptype'] = 2;
            }
            else if (this.props.title === localConstant.visit.APPROVED_VISITS_PROFORMA_NDT) {
                this.reportParam['reportname'] = EvolutionReportsAPIConfig.ApprovedVisitsReportExcelNDT;
                reportFileName = 'ApprovedVisitsReportNDT';
                this.reportParam['isNDT'] = 'true';
                this.reportParam['reptype'] = 2;
            }
            else if (this.props.title === localConstant.timesheet.APPROVED_TIMESHEET_REPORT_PROFORMA) {
                this.reportParam['reportname'] = EvolutionReportsAPIConfig.ApprovedTimesheetReport;
                reportFileName = 'ApprovedTimesheetReport';
                this.reportParam['isNDT'] = 'false';
                this.reportParam['reptype'] = 3;
            }
            else if (this.props.title === localConstant.timesheet.APPROVED_TIMESHEET_REPORT_NDT) {
                this.reportParam['reportname'] = EvolutionReportsAPIConfig.ApprovedTimesheetReportNDT;
                reportFileName = 'ApprovedTimesheetReportNDT';
                this.reportParam['isNDT'] = 'true';
                this.reportParam['reptype'] = 3;
            }
            this.reportParam['background'] = true;
            
            DownloadReportFile(this.reportParam, 'application/vnd.ms-excel', reportFileName, '.xls', this.props);
        }
    }

    handlerChange = (e) => {
        if (e.target.value != undefined && e.target.value != '') {
            let _isVisit = false;
            let _isNDT = false;
            const companyId = Array.isArray(this.props.companyList) && this.props.companyList.filter( x=>x.companyCode===this.props.selectedCompany );
            const loggedInCompanyId = companyId[0].companyId;
            if (this.props.title === localConstant.visit.APPROVED_VISITS_REPORT_PROFORMA || this.props.title === localConstant.visit.APPROVED_VISITS_PROFORMA_NDT) {
                _isVisit = true;
            }
            if (this.props.title === localConstant.visit.APPROVED_VISITS_PROFORMA_NDT || this.props.title === localConstant.timesheet.APPROVED_TIMESHEET_REPORT_NDT) {
                _isNDT = true;
            }
            if (e.target.name === "customers") {
                this.reportParam["CustomerId"] = e.target.options[e.target.selectedIndex].getAttribute('customId');
                this.props.actions.ClearCustomerContracts();
                const data = {
                    "customerCode": e.target.value,
                    "ContractHolderCompanyId":loggedInCompanyId,
                    "isVisit": _isVisit,
                    "isNDT":_isNDT
                };
                this.reportParam["ContractId"] = undefined;
                this.reportParam["ProjectId"] = undefined;
                this.props.actions.ClearCustomerContracts();
                this.props.actions.ClearContractProjects();
                this.props.actions.FetchApprovedUnApprovedVistContract(data);
            }
            else if (e.target.name === "contracts") {
                this.reportParam["ContractId"] = e.target.options[e.target.selectedIndex].getAttribute('customId');
                this.props.actions.ClearContractProjects();
                const Params1 = {
                    "contractNumber" : e.target.value,
                    "ContractHolderCompanyId":loggedInCompanyId,
                    "isApproved" : true,
                    "isVisit" : _isVisit,
                    "isOperating": false,
                    "isNDT":_isNDT,
                    "CoordinatorId": undefined
                };
                this.reportParam["ProjectId"] = undefined;
                this.props.actions.FetchContractProjects(Params1);
            }
            else if (e.target.name === "projects") {
                this.reportParam["ProjectId"] = e.target.value;
            }
        }
        else{
            if (e.target.name === "customers") {
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
            else if (e.target.name === "projects") {
                this.reportParam["ProjectId"] = undefined;
            }
        }
    }

    mergedContracts = (contracts) => {
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
        const contractValue = this.mergedContracts(this.props.contractList);
        const projectValue = this.mergedProjects(this.props.projectList);
        return (
            <Fragment>
                <Modal id="reportId"
                    title={ this.props.title }
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
                            action: this.exportVisitApprovedReportHandler,
                            btnClass: 'btn-small modal-close',
                            showbtn: true
                        }
                    ]}
                    isShowModal={this.props.showmodal}>
                    <div className="row">
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
                            optionsList={ this.props.customerData }
                            autocomplete="off"
                        />
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
                            optionsList={ contractValue }
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
                        <label class='bold'>{localConstant.supplier.FORMAT}</label>
                        <div className="ml-1 mt-1">
                        <label>
                            <input className="with-gap" defaultChecked={true}
                                value='excel' name="Format" type="radio" />
                            <span>{localConstant.commonConstants.EXCEL}</span>
                        </label>
                        </div>
                    </div>

                </Modal>

            </Fragment>
        );
    }
}

export default ApprovedVisitModal;

ApprovedVisitModal.propTypes = {
    contractList: PropTypes.array,
    projectList: PropTypes.array,
};

ApprovedVisitModal.defaultProps = {
    contractList: [],
    projectList: [],
};