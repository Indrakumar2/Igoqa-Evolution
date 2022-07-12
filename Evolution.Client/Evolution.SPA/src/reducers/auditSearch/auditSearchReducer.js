import { auditSearchActionTypes } from '../../constants/actionTypes';
const initialState = {
  moduleData: [],
  submoduleData:[]
};
export const auditSearchReducer = (state = initialState, actions) => {
  const { data, type } = actions;
  switch (type) {
    case auditSearchActionTypes.FETCH_MODULE_DATA_FOR_AUDITSEARCH:
      state = {
        ...state,
        moduleData: data,

      };
      return state;

    case auditSearchActionTypes.FETCH_SUB_MODULE_DATA:
      state = {
        ...state,
        submoduleData: data
      };
      return state;

    case auditSearchActionTypes.FETCH_PARENT_MODULE_DATA:
      return state;

    case auditSearchActionTypes.FETCH_PARENT_SUB_GRID_DATA:
      state={
        ...state,
        auditSubGridData:data
      };
      return state;
      case auditSearchActionTypes.CLEAR_MODULE_NAME:
        state={
         ...state,
  
        };
        return state;
        case auditSearchActionTypes.CLEAR_PARENT_GRID_DATA:
            state={
             ...state,  
             auditSearchData:[],
            //  auditSubGridData:[]
            };
            return state;

      case auditSearchActionTypes.FETCH_AUDIT_SEARCH_DATA:
          state = {
            ...state,
            auditSearchData: data
          };
      return state;

      case auditSearchActionTypes.FETCH_AUDIT_SEARCH_LOG_DATA:
          state = {
            ...state,
            auditSearchLogData: data
          };
      return state;

    case auditSearchActionTypes.FETCH_AUDIT_SEARCH_UNIQUE_ID:
     state = {
      ...state,
      auditSearchUnquieId: data
    };
      return state;
      
    default:
      return state;

  }

};