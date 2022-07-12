import { customerActionTypes,sideMenu } from '../../../constants/actionTypes';
const initialState = {
    customerDetail: {},
    companyDetail: {},
    customerList: [],
    showButton: false,
    isbtnDisable: true,
    editedAssignmentReference: {},
    editedAddressReference: {},
    editedContactReference: {},
    editedAccountReference: {},
    selectedCustomerCode: null,
    selectedContractNumber: null,
    customerContracts: [],
    editedNoteData: {},
    assignmentReferenceTypes: [],
    masterDocumentTypeData: [],
    copyDocumentDetails: {},
    editDocumentDetails: {},
    Loader: false,
    customerSave: false,
    customerUpdated: false,
    displayDocuments: [],
    customerContractDocumentData: [],
    customerProjectDocumentData: [],
    selectedcontractStatusOfCustomer: 'O',
    customerprojects: [],
    selectedprojectStatusOfCustomer: 'O',
    fullAddress:'',
    extranetUser:{},
    fileToBeUploaded:[]
};
export const CustomerReducer = (state = initialState, action) => {
    let prevState = null, index = -1, editedRow = {};
    const { type, data } = action, newState = [];
    switch (type) {
        /**
         * customer updated case
         */
        case customerActionTypes.IS_EXTRANET_USER_ACCOUNT_MODAL:
            state = {
                ...state,
                isExtranetUserAccountModal: data
            };
            return state;
        case customerActionTypes.CUSTOMER_UPDATED:
            state = {
                ...state,
                customerUpdated: data
            };
            return state;
        case customerActionTypes.SET_LOADER:

            state = {
                ...state,
                Loader: data,
                isbtnDisable: true
            };
            return state;
        case customerActionTypes.REMOVE_LOADER:
            state = {
                ...state,
                Loader: data,
            };
            return state;
        case customerActionTypes.FETCH_CUSTOMER_DETAIL:
            state = {
                ...state,
                customerDetail: data,
                isbtnDisable: true

            };
            return state;

        case customerActionTypes.FETCH_CUSTOMER_LIST:
            state = {
                ...state,
                customerList: data
            };
            return state;

        case customerActionTypes.ADD_CUSTOMER_ADDRESS:
            if (state.customerDetail.Addresses == null) {
                state = {
                    ...state,
                    customerDetail: {
                        ...state.customerDetail,
                        Addresses: []
                    }
                };
            }
            state = {
                ...state,
                customerDetail: {
                    ...state.customerDetail,
                    Addresses: [
                        ...state.customerDetail.Addresses, data
                    ]
                },
                isbtnDisable: false

            };
            return state;
        case customerActionTypes.ADD_CUSTOMER_CONTACT:
            newState = Object.assign([], state.customerDetail.Addresses);
            newState.map(row => {
                if (row.AddressId == data.CustomerAddressId) {
                    if (row.Contacts === null) {
                        row.Contacts = [];
                    }
                    row.Contacts.push(data);
                }
            });
            state = {
                ...state,
                customerDetail: {
                    ...state.customerDetail,
                    Addresses: newState
                },
                isbtnDisable: false

            };
            return state;
        case customerActionTypes.DELETE_CUSTOMER_ADDRESS:
            newState = Object.assign([], state.customerDetail.Addresses);
            newState.map(customerAddress => {
                data.map(row => {
                    if (customerAddress.AddressId === row.AddressId) {
                        index = newState.findIndex(value => (value.AddressId === row.AddressId));
                        if (row.RecordStatus != "N") {
                            newState[index].RecordStatus = "D";
                        }
                        else {
                            newState.splice(index, 1);
                        }
                    }
                });
            });
            state = {
                ...state,
                customerDetail: {
                    ...state.customerDetail,
                    Addresses: newState
                },
                isbtnDisable: false

            };
            return state;

        case customerActionTypes.DELETE_CUSTOMER_CONTACT:
            let newState = Object.assign([], state.customerDetail.Addresses);
            data.map(row => {
                newState.map(customerContact => {
                    customerContact.Contacts.map(contact => {
                        if (contact.ContactId === row.ContactId) {
                            index = customerContact.Contacts.findIndex(value => (value.ContactId == row.ContactId));
                            if (row.RecordStatus != "N") {
                                customerContact.Contacts[index].RecordStatus = "D";
                            }
                            else {
                                customerContact.Contacts.splice(index, 1);
                            }
                        }
                    });
                });
            });
            state = {

                ...state,
                customerDetail: {
                    ...state.customerDetail,
                    Addresses: newState
                },
                isbtnDisable: false

            };
            return state;
        case customerActionTypes.ADD_ASSIGNMENT_REFERENCE:
            if (state.customerDetail.AssignmentReferences == null) {
                state = {
                    ...state,
                    customerDetail: {
                        ...state.customerDetail,
                        AssignmentReferences: []
                    }
                };
            }
            state = {
                ...state,
                customerDetail: {
                    ...state.customerDetail,
                    AssignmentReferences: [
                        ...state.customerDetail.AssignmentReferences, data
                    ]
                },
                isbtnDisable: false

            };
            return state;
        case customerActionTypes.EDIT_ADDRESS_REFERENCE:
            state = {
                ...state,
                editedAddressReference: data,
                showButton: true,

            };
            return state;
        case customerActionTypes.EDIT_STATE_ADDRESS_REFERENCE:
            state = {
                ...state,
                editedAddressReference: data,
            };
            return state;
        case customerActionTypes.UPDATE_ADDRESS_REFERENCE:
            editedRow = Object.assign({}, state.editedAddressReference, data);
            const addressRefIndex = state.customerDetail.Addresses.findIndex(address => address.AddressId === editedRow.AddressId);
            const addressRefNewState = Object.assign([], state.customerDetail.Addresses);
            addressRefNewState[addressRefIndex] = editedRow;
            if (addressRefIndex >= 0)
                state = {
                    ...state,
                    customerDetail: {
                        ...state.customerDetail,
                        Addresses: addressRefNewState
                    },
                    isbtnDisable: false
                };
            return state;
        case customerActionTypes.EDIT_CONTACT_REFERENCE:  //contactttt changess
            state = {
                ...state,
                editedContactReference: data,
                showButton: true,
        
            };
            return state;

        case customerActionTypes.UPDATE_CONTACT_REFERENCE:
            editedRow = Object.assign({}, state.editedContactReference, data);
            const contactRefNewState = Object.assign([], state.customerDetail.Addresses);
            const contactRefIndex = -1;

            const contactaddressRefIndex = state.customerDetail.Addresses.findIndex(address => address.AddressId == editedRow.parentAddressId);
            const customercontactRefIndex = state.customerDetail.Addresses[contactaddressRefIndex].Contacts.findIndex(address => address.ContactId == editedRow.ContactId);
            contactRefNewState[contactaddressRefIndex].Contacts[customercontactRefIndex] = editedRow;

            if (contactaddressRefIndex >= 0 && customercontactRefIndex >= 0)
                state = {
                    ...state,
                    customerDetail: {
                        ...state.customerDetail,
                        Addresses: contactRefNewState,

                    },
                    isbtnDisable: false
                };
            return state;
        case customerActionTypes.EDIT_ASSIGNMENT_REFERENCE:
            state = {
                ...state,
                editedAssignmentReference: data,
                showButton: true,
                isbtnDisable: false

            };
            return state;
        case customerActionTypes.UPDATE_ASSIGNMENT_REFERENCE:
            editedRow = Object.assign({}, state.editedAssignmentReference, data);
            const assignmentRefIndex = state.customerDetail.AssignmentReferences.findIndex(note => note.customerAssignmentReferenceId === editedRow.customerAssignmentReferenceId);
            const assignmentRefNewState = Object.assign([], state.customerDetail.AssignmentReferences);
            assignmentRefNewState[assignmentRefIndex] = editedRow;
            if (assignmentRefIndex >= 0)
                state = {
                    ...state,
                    customerDetail: {
                        ...state.customerDetail,
                        AssignmentReferences: assignmentRefNewState
                    },
                    isbtnDisable: false

                };
            return state;
        case customerActionTypes.UPDATE_ACCOUNT_REFERENCE:
            editedRow = Object.assign({}, state.editedAccountReference, data);
            const accountRefIndex = state.customerDetail.AccountReferences.findIndex(note => note.customerCompanyAccountReferenceId === editedRow.customerCompanyAccountReferenceId);
            const accountRefNewState = Object.assign([], state.customerDetail.AccountReferences);
            accountRefNewState[accountRefIndex] = editedRow;
            if (accountRefIndex >= 0)
                state = {
                    ...state,
                    customerDetail: {
                        ...state.customerDetail,
                        AccountReferences: accountRefNewState
                    },
                    editedAccountReference: {},
                    isbtnDisable: false

                };
            return state;
        case customerActionTypes.DELETE_ASSIGNMENT_REFERENCE:
            prevState = Object.assign([], state.customerDetail.AssignmentReferences);
            prevState.map(assignment => {
                data.map(row => {
                    if (assignment.customerAssignmentReferenceId === row.customerAssignmentReferenceId) {
                        index = prevState.findIndex(value => (value.customerAssignmentReferenceId == row.customerAssignmentReferenceId));
                        if (row.recordStatus != "N") {
                            prevState[index].recordStatus = "D";
                        }
                        else {
                            prevState.splice(index, 1);
                        }
                    }
                });
            });
            state = {
                ...state,
                customerDetail: {
                    ...state.customerDetail,
                    AssignmentReferences: prevState
                },
                isbtnDisable: false

            };
            return state;
        case customerActionTypes.DELETE_ACCOUNT_REFERENCE:
            prevState = Object.assign([], state.customerDetail.AccountReferences);
            prevState.map(account => {
                data.map(row => {
                    if (account.customerCompanyAccountReferenceId === row.customerCompanyAccountReferenceId) {
                        index = prevState.findIndex(value => (value.customerCompanyAccountReferenceId == row.customerCompanyAccountReferenceId));
                        if (row.recordStatus != "N") {
                            prevState[index].recordStatus = "D";
                        }
                        else {
                            prevState.splice(index, 1);
                        }
                    }
                });
            });
            state = {
                ...state,
                customerDetail: {
                    ...state.customerDetail,
                    AccountReferences: prevState
                },
                isbtnDisable: false

            };
            return state;
        case customerActionTypes.ADD_ACCOUNT_REFERENCE:
            if (state.customerDetail.AccountReferences == null) {
                state = {
                    ...state,
                    customerDetail: {
                        ...state.customerDetail,
                        AccountReferences: []
                    },
                    isbtnDisable: false
                };
            }
            state = {
                ...state,
                customerDetail: {
                    ...state.customerDetail,
                    AccountReferences: [
                        ...state.customerDetail.AccountReferences, data
                    ]
                },
                isbtnDisable: false

            };
            return state;
        case customerActionTypes.EDIT_ACCOUNT_REFERENCE:
            state = {
                ...state,
                editedAccountReference: data,
                showButton: true,
                isbtnDisable: false

            };
            return state;
        case customerActionTypes.SELECTED_CUSTOMER_CODE:
            state = {
                ...state,
                selectedCustomerCode: data
            };
            return state;

        case customerActionTypes.ADD_NOTES_DETAILS:  //Notes Add
            if (state.customerDetail.Notes == null) {
                state = {
                    ...state,
                    customerDetail: {
                        ...state.customerDetail,
                        Notes: []
                    },
                    isbtnDisable: false

                };
            }
            state = {
                ...state,
                customerDetail: {
                    ...state.customerDetail,
                    Notes: [
                        ...state.customerDetail.Notes, data
                    ]
                },
                isbtnDisable: false

            };
            return state;
        case customerActionTypes.FETCH_DOCUMENT_TYPES: //Document Type Master Data            
            state = {
                ...state,
                masterDocumentTypeData: data
            };
            return state;
        case customerActionTypes.FETCH_CONTRACT_OF_CUSTOMER:  //Contracts  
            state = {
                ...state,
                customerContracts: data.responseResult,
                selectedcontractStatusOfCustomer: data.selectedValue
            };
            return state;
        case customerActionTypes.FETCH_CUSTOMER_PROJECTS:
            state = {
                ...state,
                customerprojects: data.responseResult,
                selectedprojectStatusOfCustomer: data.selectedValue
            };
            return state;
        case customerActionTypes.CUSTOMER_CONTRACT_DOCUMENTS:
            state = {
                ...state,
                customerContractDocumentData: data
            };
            return state;
        case customerActionTypes.CUSTOMER_PROJECT_DOCUMENTS:
            state = {
                ...state,
                customerProjectDocumentData: data
            };
            return state;
        case customerActionTypes.ADD_DOCUMENTS_DETAILS:  //Documents Add              
            if (state.customerDetail.Documents == null) {
                state = {
                    ...state,
                    customerDetail: {
                        ...state.customerDetail,
                        Documents: []
                    },
                    isbtnDisable: false
                };
            }
            newState = data.concat(state.customerDetail.Documents);
            state = {
                ...state,
                customerDetail: {
                    ...state.customerDetail,
                    Documents: newState
                },
                editDocumentDetails: {},
                isbtnDisable: false
            };
            return state;
        case customerActionTypes.COPY_DOCUMENTS_DETAILS: //Documents Copy        
            state = {
                ...state,
                copyDocumentDetails: data,
                isbtnDisable: false
            };
            return state;
        case customerActionTypes.DELETE_DOCUMENTS_DETAILS: //Documents Delete
            newState = Object.assign([], state.customerDetail.Documents);
            data.map(row => {
                newState.map(document => {
                    if (document.id === row.id) {
                        index = newState.findIndex(value => (value.id == row.id));
                        newState[index].recordStatus = "D";
                    }
                });
            });
            state = {
                ...state,
                customerDetail: {
                    ...state.customerDetail,
                    Documents: newState
                },
                isbtnDisable: false

            };
            return state;
        case customerActionTypes.EDIT_CUSTOMER_DOCUMENTS_DETAILS: //Edit Document
            state = {
                ...state,
                editDocumentDetails: data,
                showButton: true,
                isbtnDisable: false
            };
            return state;
        case customerActionTypes.UPDATE_DOCUMENTS_DETAILS:  //Document Update              
            state = {
                ...state,
                customerDetail: {
                    ...state.customerDetail,
                    Documents: data
                },
                isbtnDisable: false
            };
            return state;
        case customerActionTypes.SHOWBUTTON:
            state = {
                ...state,
                showButton: false,
                editDocumentDetails: {},
                editedNoteData: {},
                editedAccountReference: {},
                editedAssignmentReference: {},
                editedAddressReference: {},
                editedContactReference: {}
            };
            return state;
        case customerActionTypes.CLEAR_STATE_CITY_DATA:
            state={
                ...state,
                editedAccountReference:data
            };
            return state;

        case customerActionTypes.CLEAR_CITY_DATA:
            state={
                ...state,
                editedAccountReference:data
            };
            return state;

        case customerActionTypes.DISPLAY_FULL_ADDRESS:
            state={
                ...state,
                fullAddress:data
            };
            return state;    
        case customerActionTypes.CLEAR_SEARCH_DATA:
            state = {
                ...state,
                customerList: []
            };
            return state;
        case customerActionTypes.FETCH_ASSIGNMENT_REF_TYPES:
            state = {
                ...state,
                assignmentReferenceTypes: data,
            };
            return state;
        case customerActionTypes.SAVE_CUTOMER_DETAILS:
            state = {
                ...state,
                customerSave: data,
                isbtnDisable: true
            };
            return state;
        case customerActionTypes.DISPLAY_DOCUMENTS:
            state = {
                ...state,
                displayDocuments: data
            };
            return state;
            case sideMenu.HANDLE_MENU_ACTION:
            state ={
                ...state,
                customerList: [],
                isbtnDisable:true
            };
            return state;

            case customerActionTypes.ADD_EXTRANET_USER:
                state = {
                    ...state,
                    extranetUser: data,
                    //isbtnDisable: true
                };
                return state;
            case customerActionTypes.ASSIGN_EXTRANET_USER_TO_CONTACT: 
                const extranetUserInfo = Object.assign({}, state.extranetUser,data);
                const addressNewState = Object.assign([], state.customerDetail.Addresses); 

                const addrRefIndex = state.customerDetail.Addresses.findIndex(address => address.AddressId == extranetUserInfo.CustomerAddressId);
                const custContactRefIndex = state.customerDetail.Addresses[addrRefIndex].Contacts.findIndex(address => address.ContactId == extranetUserInfo.ContactId);
                if(addressNewState[addrRefIndex].Contacts[custContactRefIndex] && extranetUserInfo)
                {
                    let customerUserProject = [];
                    if(addressNewState[addrRefIndex].Contacts[custContactRefIndex].RecordStatus===null)
                    {
                        addressNewState[addrRefIndex].Contacts[custContactRefIndex].RecordStatus ="M";
                    } 
                    addressNewState[addrRefIndex].Contacts[custContactRefIndex].UserInfo = extranetUserInfo.UserInfo; 
                    addressNewState[addrRefIndex].Contacts[custContactRefIndex].LogonName= extranetUserInfo.UserInfo.LogonName;
                    addressNewState[addrRefIndex].Contacts[custContactRefIndex].IsPortalUser = true;  
                   if(extranetUserInfo.AuthourisedProjects)
                   {
                    extranetUserInfo.AuthourisedProjects.forEach(element => {
                        customerUserProject.push({ ProjectNumber : element.projectNumber,UserId:extranetUserInfo.UserId,RecordStatus:element.recordStatus }); 
                    }); 
                   } 
                    addressNewState[addrRefIndex].Contacts[custContactRefIndex].UserInfo.CustomerUserProjectNumbers=customerUserProject;
                    customerUserProject=[];
                } 

                if (addrRefIndex >= 0 && custContactRefIndex >= 0)
                    state = {
                        ...state,
                        customerDetail: {
                            ...state.customerDetail,
                            Addresses: addressNewState,

                        },
                        isbtnDisable: false
                    };
                return state; 
            case customerActionTypes.DEACTIVATE_EXTRANET_USER_ACCOUNT: 
                const extranetUserData = Object.assign({}, state.extranetUser,data);
                const custAddNewState = Object.assign([], state.customerDetail.Addresses); 

                const addrIdx = state.customerDetail.Addresses.findIndex(address => address.AddressId == extranetUserData.CustomerAddressId);
                const custContactIdx = state.customerDetail.Addresses[addrIdx].Contacts.findIndex(address => address.ContactId == extranetUserData.ContactId);
                if(custAddNewState[addrIdx].Contacts[custContactIdx] )
                { 
                    if(!custAddNewState[addrIdx].Contacts[custContactIdx].RecordStatus)
                    {
                        custAddNewState[addrIdx].Contacts[custContactIdx].RecordStatus ="M";
                    }
                    if(!custAddNewState[addrIdx].Contacts[custContactIdx].UserInfo.RecordStatus)
                    {
                        custAddNewState[addrIdx].Contacts[custContactIdx].UserInfo.RecordStatus ="M";
                    }  
                    custAddNewState[addrIdx].Contacts[custContactIdx].UserInfo.IsActive = false;  
                    custAddNewState[addrIdx].Contacts[custContactIdx].IsPortalUser = false; 
                } 

                if (addrIdx >= 0 && custContactIdx >= 0)
                    state = {
                        ...state,
                        customerDetail: {
                            ...state.customerDetail,
                            Addresses: custAddNewState,

                        },
                        isbtnDisable: false
                    };
                return state;
            case customerActionTypes.POPULATE_EXTRANET_USERS_LIST:
                state = {
                    ...state,
                    extranetUsers: data 
                };
                return state;
            case customerActionTypes.EDIT_NOTES_DETAILS: //D661 issue8
                    state = {
                        ...state,
                        customerDetail: {
                            ...state.customerDetail,
                            Notes: data
                        },
                        isbtnDisable: false
                    };
                return state;
                
            case customerActionTypes.ADD_FILES_TO_BE_UPLOADED: //Add files to be uploaded async            
                state = {
                        ...state,
                        fileToBeUploaded: data,
                    };
                return state;

            case customerActionTypes.CLEAR_FILES_TO_BE_UPLOADED: //clear files to be uploaded async            
                state = {
                        ...state,
                        fileToBeUploaded: [],
                    };
                return state;

        default:
            return state;
    }
};