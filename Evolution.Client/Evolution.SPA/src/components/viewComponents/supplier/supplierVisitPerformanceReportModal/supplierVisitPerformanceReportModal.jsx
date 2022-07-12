import React, { Component, Fragment } from 'react';
import {
    getlocalizeData,
    GenerateReport,
    ObjectIntoQuerySting
} from '../../../../utils/commonUtils';
import Modal from '../../../../common/baseComponents/modal';
import { DownloadReportFile }  from '../../../../common/reportUtil/ssrsUtil';
import arrayUtil from '../../../../utils/arrayUtil';
import { headerData } from '../../supplierpo/supplierpoSearch/headerData';
import { SupplierVisitSearchFields } from './supplierVisitSearchFields';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';

const localConstant = getlocalizeData();

class suppliervisitPerformanceReportModal extends Component {

    constructor(props) {
        super(props);
        this.state = {
            isSupplierPOContactOpen: false,
            isSupplierPOContactEdit: false,
            deliveryDate: '',
            completedDate: '',
            enableSupplierLink: true,
            selectedSupplier: {},
        };

        this.excelDatas = {};
        this.NcrRef = React.createRef();
    }

    supplierPopupOpen = (data) => {
        this.props.actions.ClearSupplierSearchList();
    }

    getMainSupplier = (data) => {
        const params = {
            supplierName: data.serachInput,
            country: data.selectedCountry
        };
        this.props.actions.FetchSupplierSearchList(params);
    }

    getSelectedMainSupplier = (data) => {
        if (data) {
            this.setState({
                selectedSupplier: data[0],
                enableSupplierLink: false
            });
            const params = {
                SupplierName: data[0] && data[0].supplierName,
                supplierPOMainSupplierAddress: data[0] && data[0].supplierAddress,
                supplierPOMainSupplierId: data[0] && data[0].supplierId
            };
            this.excelDatas['SupplierName'] = data[0] && data[0].supplierName;
            this.props.actions.updateSupplierDetails(params);
        }
    }

    handleSupplier = (e) => {
        if(e){
            this.excelDatas['SupplierName'] = e[0].supplierName;
        }
        else{
            this.excelDatas['SupplierName'] = undefined;
        }
    }

    handlerChange = (e) => {
        if(e.target.value != ''){
            this.excelDatas[e.target.name] = e.target.value;
        }
        else{
            this.excelDatas[e.target.name] = undefined;
        }
    }

    OnChangeSearchCustomer =(e) =>{
        if(e){
            this.excelDatas['CustomerName'] = e[0].customerName;
        }
        else{
            this.excelDatas['CustomerName'] = undefined;
        }
        this.props.actions.UpdateReportCustomer(e);
    }

    ClearReportsData =()=>{
        this.excelDatas['CustomerName'] = undefined;
        this.props.actions.ClearReportsData();
    }

    exportExcel = async (e) => {
        const SupplierName = this.excelDatas['SupplierName'];
        const CustomerName = this.excelDatas['CustomerName'];
        const CHCompanyName = this.excelDatas['CHCompanyName'];
        const OCCompanyName = this.excelDatas['OCCompanyName'];
        const AssignmentNumber = this.excelDatas['AssignmentNumber'];
        const SupplierPONumber = this.excelDatas['SupplierPONumber'];
        const ProjectNumber = this.excelDatas['ProjectNumber'];
        if(SupplierName == undefined && CustomerName == undefined && CHCompanyName == undefined && OCCompanyName == undefined && AssignmentNumber == undefined && SupplierPONumber == undefined && ProjectNumber == undefined){
            IntertekToaster(localConstant.commonConstants.ERR_REPORT, 'warningToast ssrs_report_message');
        }
        else{
            const _localStorage = localStorage.getItem('Username');
            this.excelDatas['username'] = _localStorage;
            // to do-- pass the required Parameters //
            if (this.NcrRef.current.checked === true) {  // Made Open as default checked as per D-869//
                this.excelDatas['NCR'] = 'open';
            }
            this.excelDatas['reportname'] = 'SupplierPerformanceReport';
            this.excelDatas['background'] = true;
            this.excelDatas['reptype'] = 1;
            DownloadReportFile(this.excelDatas, 'application/vnd.ms-excel', 'SupplierPerformanceReport', '.xls', this.props);
        }
    }
    
    hidemodal=()=>{
        this.props.hidemodal();
        this.props.actions.ClearCustomerData();
    }

    clearSupplierVisit=() => {
        this.excelDatas['SupplierName'] = undefined;
    }

    render() {
        const { companyList } = this.props;
        const sortedCompanyList = arrayUtil.sort(companyList, 'companyName', 'asc');
        const defaultSort = [
            {
                "colId": "supplierName",
                "sort": "asc"
            },
        ];

        return (
            <Fragment>
                <Modal id="reportId"
                    title={'Supplier Visit Performance Report'}
                    titleClassName='bold'
                    buttons={[
                        {
                            name: 'Cancel',
                            action: this.hidemodal,
                            type: "button",
                            btnClass: 'btn-flat mr-1',
                            showbtn: true
                        },
                        {
                            name: 'View',
                            type: "button",
                            action: this.exportExcel,
                            btnClass: 'btn-small modal-close',
                            showbtn: true
                        }
                    ]}
                    isShowModal={this.props.showmodal}>
                    < SupplierVisitSearchFields
                        handlerChange={this.handlerChange}
                        handleSupplier={this.handleSupplier}
                        ClearSearchResult={this.ClearSearchResult}
                        supplierList={this.props.supplierList}
                        sortedCompanyList={sortedCompanyList}
                        clearSupplierVisit={this.clearSupplierVisit}
                        OnChangeSearchCustomer={this.OnChangeSearchCustomer}
                        defaultSort={defaultSort}
                        cancelSupplierVisit={this.clearSupplierVisit}
                        supplierPopupOpen={data => this.supplierPopupOpen(data)}
                        getMainSupplier={data => this.getMainSupplier(data)}
                        getSelectedMainSupplier={data => this.getSelectedMainSupplier(data)}
                        headerData={headerData}
                        NcrRef={this.NcrRef}
                        reportsCustomerName={this.props.reportsCustomerName}
                        ClearReportsData={ this.ClearReportsData }
                        defaultReportCustomerName={this.props.defaultReportCustomerName }
                    />

                </Modal>
            </Fragment>
        );
    }
}

export default suppliervisitPerformanceReportModal;