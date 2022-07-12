import CustomModal from './customModal';
import { connect } from 'react-redux';

const mapStateToProps = (state) => {
    return {
        modalData:state.CustomModalReducer
    };
};

export default connect(mapStateToProps)(CustomModal);
// export default CustomModal;
