import { contractActionTypes } from '../../constants/actionTypes';
const actions = {
    AddContractNotesDetails: (payload) => ({
        type: contractActionTypes.ADD_CONTRACT_NOTE,
        data: payload
    }),
    GetSelectedContractNumber: (payload) => ({
        type: contractActionTypes.SELECTED_CONTRACT_NUMBER,
        data: payload
    }),
    ShowButtonHandler: () => ({
        type: contractActionTypes.SHOWBUTTON
    })
};
export const GetSelectedContractNumber = (data) => (dispatch) => {
    dispatch(actions.GetSelectedContractNumber(data));
};

export const AddContractNotesDetails = (data) => (dispatch) => {
    dispatch(actions.AddContractNotesDetails(data));
};

export const ShowButtonHandler = () => (dispatch) => {
    dispatch(actions.ShowButtonHandler());
};