import { techSpecActionTypes } from '../../constants/actionTypes';
export const WonLostReducer = (state, action) => {
    const { type, data } = action;
    switch (type) {

        case techSpecActionTypes.FETCH_WONLOST_DATAS:
            state={
                ...state,
                wonLostDatas:data
            };
            return state;
        case techSpecActionTypes.HANDLE_MENU_ACTION:
            state={
                ...state,
                wonLostDatas:{}
            };
            return state;

        default:
            return state;
    }
};