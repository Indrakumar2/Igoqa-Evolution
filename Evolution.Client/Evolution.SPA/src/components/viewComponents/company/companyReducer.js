import { companyActionTypes,sideMenu } from '../../../constants/actionTypes';
import { getlocalizeData, isEmpty,isEmptyReturnDefault } from '../../../utils/commonUtils';
import _ from 'lodash';

const localConstant = getlocalizeData();
const initialState = {
    companyDetail: {
        CompanyInvoiceInfo:{
            invoiceRemittances: [],
            invoiceFooters: []
        }
    },
    companyDataList: [],
    isopen: true,
    isSearch: false,
    showButton: false,
    isbtnDisable: true,
    editedcompanyOffice: {},
    selectedCompanyCode: null,
    payroll: [],
    notes: [],
    cityMasterData: [],
    buisnessUnitMasterData: [],
    salutationMasterData: [],
    taxMasterData: [],
    vatPrefixMasterData: [],
    companyVatPrefixMasterData: [],
    stateMasterData: [],
    countryMasterData: [],
    companyDocuments: [],
    companyNotes: [],
    expectedMargin: [],
    companyEmail: [],
    documentsToUpdate: [],
    selectedCustomerCode: null,
    selectedContractNumber: null,
    companyFilteredContracts: null,
    companyContracts: null,
    editedNoteData: {},
    masterDocumentTypeData: [],
    copyDocumentDetails: {},
    editCompanyDocumentDetails: {},
    editExpectedMarginDetails: {},
    editCompanyTaxes: {},
    //Division cost centre margin starts
    isEditCompanyDivisionCostCenterUpdate: false,
    editCompanyDivisionCostCenter: {},
    isEditCompanyDivision: false,
    //Division cost centre margin Ends
    CompanyPayrolls: [],
    CompanyPayrollPeriods: [],
    isEditCompanyPayroll: false,
    editInvoiceRemittance: {},
    editInvoiceFooter: {},
    displayDocuments: [],

    //Payroll starts
    CostOfSaleReference: [],
    editPayrollPeriodName: {},
    //Payroll Ends

    //company save
    companySave: false,
    companyUpdated: false,
    companyLogo: [],
    selectedContractStatus: 'O',
    selectedPayrollData:{},
    selectedDivisionData:{},
    fileToBeUploaded:[]
};

export const CompanyReducer = (state = initialState, action) => {
    const { type, data } = action;
    let editedRow = {}, index = -1, newState = {};
    switch (type) {
        case companyActionTypes.SHOW_HIDE_PANEL:
            state = {
                ...state,
                isopen: !state.isopen,
                isSearch: false

            };
            return state;
        case companyActionTypes.COMPANY_UPDATED:
            state = {
                ...state,
                companyUpdated: data,
                //isbtnDisable:true,
            };
            return state;
        case companyActionTypes.FETCH_COMPANY_DATALIST:
            state = {
                ...state,
                companyDataList: data,
                isopen: false,
                isSearch: true

            };
            return state;

        case companyActionTypes.SELECTED_COMPANY_CODE:
            state = {
                ...state,
                selectedCompanyCode: data,
                isbtnDisable: true,
            };
            return state;
        case companyActionTypes.CLEAR_SEARCH_DATA:
            state = {
                ...state,
                companyDataList: [],
                isopen: true
            };
            return state;

        case companyActionTypes.FETCH_COMPANY_DATA:
            state = {
                ...state,
                companyOffices: {
                    ...state.companyOffices,
                    Offices: data
                }
            };
            return state;
        case companyActionTypes.FETCH_COMPANY_EMAIL: //company email
            state = {
                ...state,
                companyEmail: data,
                isbtnDisable: false,
            };
            return state;

        case companyActionTypes.ADD_EXPECTED_MARGIN:
            if (state.companyDetail.CompanyExpectedMargins == null) {
                state = {
                    ...state,
                    companyDetail: {
                        ...state.companyDetail,
                        CompanyExpectedMargins: []
                    },
                    isbtnDisable: false

                };
            }
            state = {
                ...state,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyExpectedMargins: [
                        ...state.companyDetail.CompanyExpectedMargins, data
                    ]
                },
                isbtnDisable: false,

            };
            return state;
        case companyActionTypes.DELETE_EXPECTED_MARGIN:
            newState = Object.assign([], state.companyDetail.CompanyExpectedMargins);
            data.forEach(row => {
                newState.forEach((margin, index) => { //D800 - #8
                    if (margin.companyExpectedMarginId === row.companyExpectedMarginId) {
                        index = newState.findIndex(value => (value.companyExpectedMarginId === row.companyExpectedMarginId));
                        if (row.recordStatus !== "N") {
                            newState[index].recordStatus = "D";
                        }
                        else {
                            newState.splice(index, 1);
                        }
                    }
                });
            });
            state = {
                ...state,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyExpectedMargins: newState
                },
                isbtnDisable: false,
            };
            return state;
        case companyActionTypes.ADD_COMPANY_OFFICE:
            if (state.companyDetail.CompanyOffices == null) {
                state = {
                    ...state,
                    companyDetail: {
                        ...state.companyDetail,
                        CompanyOffices: []
                    },
                    isbtnDisable: false,
                };
            }
            state = {
                ...state,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyOffices: [
                        ...state.companyDetail.CompanyOffices, data
                    ]
                }, isbtnDisable: false

            };
            return state;
        case companyActionTypes.UPDATE_COMPANY_OFFICE:
            index = state.companyDetail.CompanyOffices.findIndex(companyOffice => companyOffice.addressId === data.addressId);
            newState = Object.assign([], state.companyDetail.CompanyOffices);
            newState[index] = data;
            if (index >= 0)
                state = {
                    ...state,
                    companyDetail: {
                        ...state.companyDetail,
                        CompanyOffices: newState,
                    },
                    isbtnDisable: false
                };
            return state;
        case companyActionTypes.DELETE_COMPANY_OFFICES:
            newState = Object.assign([], state.companyDetail.CompanyOffices);
            data.forEach(row => {
                newState.forEach((companyOffice, index) => { //D800 - #8
                    if (companyOffice.addressId === row.addressId) {
                        index = newState.findIndex(value => (value.addressId === row.addressId));
                        if (row.recordStatus !== "N") {
                            newState[index].recordStatus = "D";
                        }
                        else {
                            newState.splice(index, 1);
                        }
                    }
                });
            });
            state = {
                ...state,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyOffices: newState
                }, isbtnDisable: false
            };
            return state;
        case companyActionTypes.EDIT_COMPANY_OFFICE:
            state = {
                ...state,
                editedcompanyOffice: data,
                showButton: true,

            };
            return state;
        case companyActionTypes.CITY:
            state = {
                ...state,
                cityMasterData: data,

            };
            return state;

        case companyActionTypes.BUSINESS_UNIT:
            state = {
                ...state,
                buisnessUnitMasterData: data
            };
            return state;
        case companyActionTypes.SALUTATION:
            state = {
                ...state,
                salutationMasterData: data
            };
            return state;
        case companyActionTypes.VAT_PREFIX:
            state = {
                ...state,
                vatPrefixMasterData: data
            };
            return state;
        case companyActionTypes.TAX:
            state = {
                ...state,
                taxMasterData: data
            };
            return state;
        case companyActionTypes.COMPANY_VAT_PREFIX:
            state = {
                ...state,
                companyVatPrefixMasterData: data,

            };
            return state;
        case companyActionTypes.COUNTRY:
            state = {
                ...state,
                countryMasterData: data,

            };
            return state;
        case companyActionTypes.STATE:
            state = {
                ...state,
                stateMasterData: data,

            };
            return state;
        case companyActionTypes.SHOWBUTTON:
            state = {
                ...state,
                showButton: false,
                editCompanyDocumentDetails: {},
                editedNoteData: {},
                editedcompanyOffice: {},
                editInvoiceRemittance: {},
                editInvoiceFooter: {},
                editExpectedMarginDetails: {},
                editCompanyTaxes: {}
            };
            return state;
        case companyActionTypes.FETCH_COMPANY_CONTRACT:  //Contracts    
            state = {
                ...state,
                companyContracts: data.responseResult,
                selectedContractStatus: data.selectedValue
            };
            return state;
        case companyActionTypes.SELECTED_CONTRACT_NUMBER:
            state = {
                ...state,
                selectedCustomerCode: data,
                isbtnDisable: false,
            };
            return state;
        case companyActionTypes.FETCH_DOCUMENT_TYPES: //Document Type Master Data            
            state = {
                ...state,
                masterDocumentTypeData: data,

            };
            return state;
        case companyActionTypes.ADD_COMPANY_DOCUMENTS_DETAILS:  //Documents Add              
            if (state.companyDetail.CompanyDocuments == null) {
                state = {
                    ...state,
                    companyDetail: {
                        ...state.companyDetail,
                        CompanyDocuments: []
                    },
                    isbtnDisable: false,
                };
            }
            newState = data.concat(state.companyDetail.CompanyDocuments);
            
            state = {
                ...state,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyDocuments: newState
                },
                isbtnDisable: false,
                editCompanyDocumentDetails: {}
            };

            return state;
        case companyActionTypes.COPY_DOCUMENTS_DETAILS: //Documents Copy        

            state = {
                ...state,
                copyDocumentDetails: data,
                isbtnDisable: false,
            };
            return state;
        case companyActionTypes.DELETE_DOCUMENTS_DETAILS: //Documents Delete 
            newState = Object.assign([], state.companyDetail.CompanyDocuments);
            data.map(row => {
                newState.map((document,index) => {
                    if (document.id === row.id) {
                       // const index = newState.findIndex(value => (value.id === row.id));
                        newState[index].recordStatus = "D";
                    }
                });
            });
            state = {
                ...state,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyDocuments: newState
                },
                copyDocumentDetails: [],
                isbtnDisable: false,
            };
            return state;
        case companyActionTypes.EDIT_EXPECTED_MARGIN: //Edit Expected margin
            state = {
                ...state,
                editExpectedMarginDetails: data,
                showButton: true
            };
            return state;
        case companyActionTypes.UPDATE_EXPECTED_MARGIN:  //expected margin Update
            const editedMarginRow = Object.assign({}, state.editExpectedMarginDetails, data);
            index = state.companyDetail.CompanyExpectedMargins.findIndex(margin => margin.companyExpectedMarginId === editedMarginRow.companyExpectedMarginId);
            newState = Object.assign([], state.companyDetail.CompanyExpectedMargins);
            newState[index] = editedMarginRow;
            if (index >= 0)
                state = {
                    ...state,
                    companyDetail: {
                        ...state.companyDetail,
                        CompanyExpectedMargins: newState
                    },
                    isbtnDisable: false,

                };
            return state;

        case companyActionTypes.EDIT_COMPANY_DOCUMENTS_DETAILS: //Edit Document

            state = {
                ...state,
                editCompanyDocumentDetails: data,
                showButton: true,
            };
            return state;
        case companyActionTypes.UPDATE_COMPANY_DOCUMENTS_DETAILS:  //Document Update            
            state = {
                ...state,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyDocuments: data
                },                
                isbtnDisable: false,
            };
            return state;
        case companyActionTypes.DISPLAY_COMPANY_DOCUMENTS:
            state = {
                ...state,
                displayDocuments: data
            };
            return state;
        case companyActionTypes.FETCH_COMPANY_DETAILS:
            state = {
                ...state,
                companyDetail: data,
                isbtnDisable: true,
            };
            return state;
        case companyActionTypes.ADD_INVOICE_REMITTANCE:
            if (isEmpty(state.companyDetail.CompanyInvoiceInfo)) {
                state = {
                    ...state,
                    companyDetail: {
                        ...state.companyDetail,
                        CompanyInvoiceInfo: {
                            ...state.companyDetail.CompanyInvoiceInfo,
                            invoiceRemittances: [],
                            invoiceFooters: []
                        }
                    },
                    isbtnDisable: false,
                };
            }

            const invoiceRemittances = isEmptyReturnDefault(state.companyDetail.CompanyInvoiceInfo.invoiceRemittances);

            state = {
                ...state,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyInvoiceInfo: {
                        ...state.companyDetail.CompanyInvoiceInfo,
                        invoiceRemittances: [
                            ...invoiceRemittances, data
                        ]
                    }
                },
                isbtnDisable: false,
            };
            return state;
        case companyActionTypes.EDIT_INVOICE_REMITTANCE:
            state = {
                ...state,
                editInvoiceRemittance: data,
                showButton: true
            };
            return state;
        case companyActionTypes.UPDATE_INVOICE_REMITTANCE:
            if (state.companyDetail.CompanyInvoiceInfo.invoiceRemittances) {
                index = state.companyDetail.CompanyInvoiceInfo.invoiceRemittances.findIndex(remittance => remittance.msgIdentifier === data.oldIdentifier);
                newState = Object.assign([], state.companyDetail.CompanyInvoiceInfo.invoiceRemittances);
                newState[index] = data;
                state = {
                    ...state,
                    companyDetail: {
                        ...state.companyDetail,
                        CompanyInvoiceInfo: {
                            ...state.companyDetail.CompanyInvoiceInfo,
                            invoiceRemittances: newState
                        }
                    },
                    isbtnDisable: false,
                };
            }
            return state;
        case companyActionTypes.DELETE_INVOICE_REMITTANCE:
            newState = Object.assign([], state.companyDetail.CompanyInvoiceInfo.invoiceRemittances);
            const companyInvoiceInfo = Object.assign({},state.companyDetail.CompanyInvoiceInfo);
            const invoiceInfoRecordStatus = companyInvoiceInfo.recordStatus !== 'N' ? 'M': 'N';

            //Finding those identifiers which is marked as deleted
            const msgIdentifiers = data.map(row => row.msgIdentifier);

            //Finding those row which status need to be marked as Delete
            const recordStatusToBeChange = newState.filter(item => msgIdentifiers.includes(item.msgIdentifier));
            const modifedRecord = recordStatusToBeChange.filter(item => item.recordStatus !== 'N').map(x => { x.recordStatus = 'D'; return x; });;

            //Eliminating all the records which was marked as deleted from the new state
            newState = newState.filter(item => !msgIdentifiers.includes(item.msgIdentifier));

            //Concatinationg the record which status has changed to deleted.
            newState = newState.concat(modifedRecord);

            state = {
                ...state,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyInvoiceInfo: {
                        ...state.companyDetail.CompanyInvoiceInfo,
                        invoiceRemittances: newState,
                        recordStatus: invoiceInfoRecordStatus
                    }
                },
                isbtnDisable: false,
            };
            return state;
        case companyActionTypes.ADD_INVOICE_FOOTER:
            if (isEmpty(state.companyDetail.CompanyInvoiceInfo)) {
                state = {
                    ...state,
                    companyDetail: {
                        ...state.companyDetail,
                        CompanyInvoiceInfo: {
                            ...state.companyDetail.CompanyInvoiceInfo,
                            invoiceFooters: []
                        }
                    },
                    isbtnDisable: false,
                };
            }
            const invoiceFooters = isEmptyReturnDefault(state.companyDetail.CompanyInvoiceInfo.invoiceFooters);
            state = {
                ...state,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyInvoiceInfo: {
                        ...state.companyDetail.CompanyInvoiceInfo,
                        invoiceFooters: [
                            ...invoiceFooters, data
                        ]
                    }
                },
                isbtnDisable: false,

            };
            return state;
        case companyActionTypes.EDIT_INVOICE_FOOTER:
            state = {
                ...state,
                editInvoiceFooter: data,
                showButton: true
            };
            return state;
        case companyActionTypes.UPDATE_INVOICE_FOOTER:
            if (state.companyDetail.CompanyInvoiceInfo.invoiceFooters) {
                index = state.companyDetail.CompanyInvoiceInfo.invoiceFooters.findIndex(footer => footer.msgIdentifier === data.oldIdentifier);
                newState = Object.assign([], state.companyDetail.CompanyInvoiceInfo.invoiceFooters);
                newState[index] = data;
                state = {
                    ...state,
                    companyDetail: {
                        ...state.companyDetail,
                        CompanyInvoiceInfo: {
                            ...state.companyDetail.CompanyInvoiceInfo,
                            invoiceFooters: newState
                        }
                    },
                    isbtnDisable: false,
                };
            }
            return state;
            
        case companyActionTypes.DELETE_INVOICE_FOOTER:
            newState = Object.assign([], state.companyDetail.CompanyInvoiceInfo.invoiceFooters);
            const compInvoiceInfo = Object.assign({},state.companyDetail.CompanyInvoiceInfo);
            const invoiceRecordStatus = compInvoiceInfo.recordStatus !== 'N' ? 'M': 'N';

            //Finding those identifiers which is marked as deleted
            const footerMsgIdentifiers = data.map(row => row.msgIdentifier);

            //Finding those row which status need to be marked as Delete
            const footerRecordStatusToBeChange = newState.filter(item => footerMsgIdentifiers.includes(item.msgIdentifier));
            const footerModifedRecord = footerRecordStatusToBeChange.filter(item => item.recordStatus !== 'N').map(x => { x.recordStatus = 'D'; return x; });;

            //Eliminating all the records which was marked as deleted from the new state
            newState = newState.filter(item => !footerMsgIdentifiers.includes(item.msgIdentifier));

            //Concatinationg the record which status has changed to deleted.
            newState = newState.concat(footerModifedRecord);

            state = {
                ...state,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyInvoiceInfo: {
                        ...state.companyDetail.CompanyInvoiceInfo,
                        invoiceFooters: newState,
                        recordStatus: invoiceRecordStatus
                    }
                },
                isbtnDisable: false,
            };
            return state;
        case companyActionTypes.FETCH_COMPANY_DIVISION:
            state = {
                ...state,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyDivisions: data
                }

            };
            return state;
        case companyActionTypes.EDIT_COMPANY_DIVISION_COST_CENTER:
            state = {
                ...state,
                editCompanyDivisionCostCenter: data,
                isbtnDisable: false,
            };
            return state;
        case companyActionTypes.UPDATE_COMPANY_DIVISION_COST_CENTER:
            state = {
                ...state,
                companyUpdated: true,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyDivisionCostCenters: data,
                },
                isbtnDisable: false,
            };
            return state;
        case companyActionTypes.UPDATE_COMPANY_DIVISION:
            state = {
                ...state,
                companyUpdated: true,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyDivisions: data
                }, isbtnDisable: false,
            };
            return state;
        case companyActionTypes.DELETE_COMPANY_DIVISION:
            state = {
                ...state,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyDivisions: data
                }, isbtnDisable: false,
            };
            return state;
        case companyActionTypes.UPDATE_COMPANY_DIVISION_BUTTON:
            state = {
                ...state,
                isEditCompanyDivision: data,
                // isbtnDisable: false,
            };
            return state;
        case companyActionTypes.UPDATE_COST_CENTRE_BUTTON:
            state = {
                ...state,
                isEditCompanyDivisionCostCenterUpdate: data,
                // isbtnDisable: false,
            };
            return state;
        case companyActionTypes.DELETE_DIVISION_COST_CENTRE:
            state = {
                ...state,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyDivisionCostCenters: data
                }, isbtnDisable: false,
            };
            return state;
        case companyActionTypes.FETCH_COMPANY_NOTE: //Notes 
            state = {
                ...state,
                companyNotes: data
            };
            return state;

        case companyActionTypes.FETCH_DIVISION_COST_CENTER:
            state = {
                ...state,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyDivisionCostCenters: data
                }
            };
            return state;

        case companyActionTypes.ADD_NEW_DIVISION:
            if (state.companyDetail.CompanyDivisions == null) {
                state = {
                    ...state,
                    companyUpdated: true,
                    companyDetail: {
                        ...state.companyDetail,
                        CompanyDivisions: []
                    },
                    isbtnDisable: false,
                };
            }
            state = {
                ...state,
                companyUpdated: true,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyDivisions: [
                        ...state.companyDetail.CompanyDivisions, data
                    ]
                },
                isbtnDisable: false,
            };
            return state;

        case companyActionTypes.ADD_NEW_DIVISION_COST_CENTRE:
            if (state.companyDetail.CompanyDivisionCostCenters == null) {
                state = {
                    ...state,
                    companyUpdated: true,
                    companyDetail: {
                        ...state.companyDetail,
                        CompanyDivisionCostCenters: []
                    }, isbtnDisable: false,
                };
            }
            state = {
                ...state,
                companyUpdated: true,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyDivisionCostCenters: [
                        ...state.companyDetail.CompanyDivisionCostCenters, data
                    ]
                },
                isbtnDisable: false,
            };
            return state;

        case companyActionTypes.UPDATE_DIVISION_NAME_IN_COST_CENTER:
            state = {
                ...state,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyDivisionCostCenters: data
                }
            };
            return state;

        case companyActionTypes.ADD_COMPANY_NOTE:  //Notes Add

            if (state.companyDetail.CompanyNotes == null) {
                state = {
                    ...state,
                    companyDetail: {
                        ...state.companyDetail,
                        CompanyNotes: []
                    },
                    isbtnDisable: false
                };
            }
            state = {
                ...state,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyNotes: [
                        ...state.companyDetail.CompanyNotes, data
                    ]
                },
                isbtnDisable: false,
            };
            return state;
        case companyActionTypes.DELETE_COMPANY_NOTE:  //Notes Delete

            newState = Object.assign([], state.companyDetail.CompanyNotes);
            data.map(row => {
                newState.map(note => {
                    if (note.companyNoteId === row.companyNoteId) {
                        const index = newState.findIndex(value => (value.companyNoteId === row.companyNoteId));
                        if (row.recordStatus !== "N") {
                            newState[index].recordStatus = "D";
                        }
                        else {
                            newState.splice(index, 1);
                        }
                    }
                });
            });
            state = {
                ...state,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyNotes: newState
                },
                isbtnDisable: false,
            };
            return state;
        case companyActionTypes.UPDATE_COMPANY_NOTE:  //Notes Update        
            const editRow = Object.assign({}, state.editedNoteData, data);
            index = state.companyDetail.CompanyNotes.findIndex(note => (note.companyNoteId === editRow.companyNoteId));
            newState = Object.assign([], state.companyDetail.CompanyNotes);
            newState[index] = editRow;
            if (index >= 0)
                state = {
                    ...state,
                    companyDetail: {
                        ...state.companyDetail,
                        CompanyNotes: newState
                    },
                    isbtnDisable: false,
                };
            return state;
        case companyActionTypes.EDIT_COMPANY_NOTE: //Notes Edit Data //D661 issue8
            state = {
                ...state,
                companyDetail: {
                     ...state.companyDetail,
                     CompanyNotes: data
                },
            isbtnDisable: false
        };
    return state;

        //company payroll manupulation
        case companyActionTypes.FETCH_PAYROLL_DATA:
            if (state.companyDetail.CompanyPayrolls == null) {
                state = {
                    ...state,
                    companyDetail: {
                        ...state.companyDetail,
                        CompanyPayrolls: []
                    },

                };
            }
            state = {
                ...state,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyPayrolls: data
                },

            };
            return state;

        case companyActionTypes.FETCH_PAYROLL_PERIOD_NAME:
            if (state.companyDetail.CompanyPayrollPeriods === null) {
                state = {
                    ...state,
                    companyDetail: {
                        ...state.companyDetail,
                        CompanyPayrollPeriods: []
                    },
                };

            } else {
                state = {
                    ...state,
                    companyDetail: {
                        ...state.companyDetail,
                        CompanyPayrollPeriods: data
                    },
                };
            }
            return state;

        case companyActionTypes.ADD_NEW_PAYROLL:
            if (state.companyDetail.CompanyPayrolls == null) {
                state = {
                    ...state,
                    companyUpdated: true,
                    companyDetail: {
                        ...state.companyDetail,
                        CompanyPayrolls: []
                    },
                    isbtnDisable: false,
                };
            }

            state = {
                ...state,
                companyUpdated: true,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyPayrolls: [
                        ...state.companyDetail.CompanyPayrolls, data
                    ]
                }, isbtnDisable: false,
            };
            return state;

        case companyActionTypes.ADD_NEW_PAYROLL_PERIOD_NAME:
            if (state.companyDetail.CompanyPayrollPeriods === null) {
                state = {
                    ...state,
                    companyUpdated: true,
                    companyDetail: {
                        ...state.companyDetail,
                        CompanyPayrollPeriods: []
                    }, isbtnDisable: false,
                };

            }
            state = {
                ...state,
                companyUpdated: true,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyPayrollPeriods: [
                        ...state.companyDetail.CompanyPayrollPeriods, data
                    ]
                }, isbtnDisable: false,
            };

            return state;

        case companyActionTypes.UPDATE_PAYROLL_PERIOD_NAME_IN_PAYROLL_PERIOD:
            state = {
                ...state,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyPayrollPeriods: data
                }
            };

            return state;

        case companyActionTypes.DELETE_PAYROLL_PERIOD_NAME:
            state = {
                ...state,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyPayrollPeriods: data
                },
                isbtnDisable: false,
            };
            return state;

        case companyActionTypes.UPADTE_PAYROLL_PERIOD_NAME:
            state = {
                ...state,
                companyUpdated: true,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyPayrollPeriods: data
                },
                isbtnDisable: false,
            };
            return state;

        case companyActionTypes.TOGGLE_SUBMIT_BUTTON:
            state = {
                ...state,
                showButton: data
            };
            return state;
        case companyActionTypes.UPDATE_OVERRIDE_COST_SALE_REFERENCE:
            state = {
                ...state,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyInfo: {
                        ...state.companyDetail.CompanyInfo,
                        isCOSEmailOverrideAllow: data
                    }
                },

                // CompanyInfo:{
                //     ...state.CompanyInfo,
                //     isCOSEmailOverrideAllow:data
                // },
                isbtnDisable: false,
            };
            return state;
        case companyActionTypes.UPDATE_AVG_TS_HOURLY_COST:
            state = {
                ...state,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyInfo: {
                        ...state.companyDetail.CompanyInfo,
                        avgTSHourlyCost: data
                    }
                },
                isbtnDisable: false,
            };
            return state;
        case companyActionTypes.DELETE_PAYROLL_TYPE:
            state = {
                ...state,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyPayrolls: data
                },
                isbtnDisable: false,
            };
            return state;
        case companyActionTypes.PAYROLL_POPUP_CLEAR:
            state = {
                ...state,
                editCompanyPayrollType: {},
                isbtnDisable: false,
            };
            return state;
        case companyActionTypes.UPDATE_COMPANY_PAYROLL_BUTTON:
            state = {
                ...state,
                isEditCompanyPayroll: data

            };
            return state;
        case companyActionTypes.UPDATE_COMPANY_PAYROLL:
            state = {
                ...state,
                companyUpdated: true,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyPayrolls: data
                },
                isbtnDisable: false
            };
            return state;
        case companyActionTypes.FETCH_COST_SALE_REFERENCE:
            state = {
                ...state,
                CostOfSaleReference: data
            };
            return state;

        case companyActionTypes.EDIT_PAYROLL_PERIOD_NAME:
            state = {
                ...state,
                editPayrollPeriodName: data,
                isbtnDisable: false,
            };
            return state;
        case companyActionTypes.UPDATE_EMAIL_TEMPLATE:
            state = {
                ...state,
                emailTemplate: data,
                isbtnDisable: false,
            };
            return state;
        case companyActionTypes.UPDATE_EMAIL_TEMPLATE_TYPE:
            state = {
                ...state,
                emailTemplateType: data,
                isbtnDisable: false,
            };
            return state;
        case companyActionTypes.UPDATE_COMPANY_EMAIL_TEMPLATE:
            let emailType = '';  
            switch (data.emailType) {
                case companyActionTypes.ONE: emailType = localConstant.companyDetails.emailTemplate.CUSTOMER_REPORTING_NOTIFICATION_TEXT;
                    break;
                case companyActionTypes.TWO: emailType = localConstant.companyDetails.emailTemplate.CUSTOMER_DIRECT_REPORTING_TEXT;
                    break;
                case companyActionTypes.THREE: emailType = localConstant.companyDetails.emailTemplate.REJECT_VISIT_TIMESHEET_TEXT;
                    break;
                case companyActionTypes.FOUR: emailType = localConstant.companyDetails.emailTemplate.VIST_COMPLETED_COORDINATOR_TEXT;
                    break;
                case companyActionTypes.FIVE: emailType = localConstant.companyDetails.emailTemplate.INTER_COMPANY_OPERATING_EMAIL;
                    break;
                default: break;
            }
            if (emailType) { _.set(state.companyDetail.CompanyEmailTemplates, emailType, data.template,data.recordStatus);
            _.set(state.companyDetail.CompanyEmailTemplates,'recordStatus','M');
        }  //company validation
            
           state = {
                ...state,
                isbtnDisable: false,
                // emailTemplate:data.template,//company validation issues //Commented for Defect 881
            };
            return state;

        case companyActionTypes.FETCH_PLACEHOLDERS: //Fetch select box options that shd be included in the editor
            state = {
                ...state,
                editorPlaceholders: data,

            };
            return state;
        //Company Notes
        case companyActionTypes.ADD_COMPANY_TAXES:
            if (state.companyDetail.CompanyTaxes == null) {
                state = {
                    ...state,
                    companyDetail: {
                        ...state.companyDetail,
                        CompanyTaxes: []
                    },
                    isbtnDisable: false,
                };
            }
            state = {
                ...state,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyTaxes: [
                        ...state.companyDetail.CompanyTaxes, data
                    ]
                },
                isbtnDisable: false,
            };
            return state;

        case companyActionTypes.EDIT_COMPANY_TAXES:
            state = {
                ...state,
                editCompanyTaxes: data,
                showButton: true,
            };
            return state;

        case companyActionTypes.UPDATE_COMPANY_TAXES:
            editedRow = Object.assign({}, state.editCompanyTaxes, data);
            index = state.companyDetail.CompanyTaxes.findIndex(tax => tax.companyTaxId === editedRow.companyTaxId);
            newState = Object.assign([], state.companyDetail.CompanyTaxes);
            newState[index] = editedRow;
            if (index >= 0)
                state = {
                    ...state,
                    companyDetail: {
                        ...state.companyDetail,
                        CompanyTaxes: newState
                    },
                    isbtnDisable: false,
                };
            return state;

        case companyActionTypes.DELETE_COMPANY_TAXES:
            newState = Object.assign([], state.companyDetail.CompanyTaxes);
            newState.map(tax => {
                data.map(row => {
                    if (tax.companyTaxId === row.companyTaxId) {
                        const index = newState.findIndex(value => (value.companyTaxId === row.companyTaxId));
                        if (row.recordStatus !== "N") {
                            newState[index].recordStatus = "D";
                        }
                        else {
                            newState.splice(index, 1);
                        }
                    }
                });
            });
            state = {
                ...state,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyTaxes: newState
                }, isbtnDisable: false,
            };
            return state;

        //Invoice Defaults
        case companyActionTypes.ADD_UPDATE_INVOICE_DEFAULTS:
            let modifiedData = Object.assign({}, state.companyDetail.CompanyInvoiceInfo, data);
            state = {
                ...state,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyInvoiceInfo: modifiedData
                }, isbtnDisable: false
            };
            return state;

        //Company Info
        case companyActionTypes.ADD_UPDATE_COMPANY_INFO:
            modifiedData = Object.assign({}, state.companyDetail.CompanyInfo, data);
            state = {
                ...state,
                companyDetail: {
                    ...state.companyDetail,
                    CompanyInfo: modifiedData
                }, isbtnDisable: false,
            };
            return state;

        case companyActionTypes.CLEAR_STATE_CITY_DATA:
            state = {
                ...state,
                stateMasterData: [],
                cityMasterData: []
            };
            return state;

            case companyActionTypes.CLEAR_CITY_DATA:
            state = {
                ...state,
                cityMasterData: []
            };
            return state;   

        case companyActionTypes.CLEAR_COMPANY_DETAILS:
            state = {
                ...state,
                companyDetail: {},
                emailTemplate : "",
                emailTemplateType : "",
                isopen: true
            };
            return state;
        case companyActionTypes.FETCH_COMPANY_LOGO_MASTER_DATA:
            state = {
                ...state,
                companyLogo: data
            };
            return state;
        case companyActionTypes.SAVE_COMPANY_DETAILS:
            state = {
                ...state,
                companySave: data,
                isbtnDisable: true,
            };
            return state;
            case sideMenu.HANDLE_MENU_ACTION:
            state ={
                ...state,
                companyDataList: [],
                isbtnDisable:true
            };
            return state;
        case companyActionTypes.UPDATE_SELECTED_PAYROLL_DATA:
            state ={
                ...state,
                selectedPayrollData:data
            };
            return state;
        case companyActionTypes.UPDATE_SELECTED_DIVISION_DATA:
            state ={
                ...state,
                selectedDivisionData:data
            };
            return state;
        case companyActionTypes.ADD_FILES_TO_BE_UPLOADED: //Add files to be uploaded async            
            state = {
                ...state,
                fileToBeUploaded: data,
            };
            return state;

        case companyActionTypes.CLEAR_FILES_TO_BE_UPLOADED: //Add files to be uploaded async            
            state = {
                ...state,
                fileToBeUploaded: [],
            };
            return state;

        default:
            return state;
    }
};