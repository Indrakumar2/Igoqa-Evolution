import Modal from './modal';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { ShowModalPopup, HideModalPopup } from './modalAction';
import { bindActionCreators } from 'redux';

const mapStateToProps = (state) => {
    return {
         showModal:state.ModalReducer.showModal
    };
}; 
const mapDispatchToProps = dispatch => {
    return {
      actions: bindActionCreators(
        {          
            ShowModalPopup,
            HideModalPopup
        }, 
        dispatch
      ),
    };
  };
export default withRouter(connect(mapStateToProps,mapDispatchToProps)(Modal));
