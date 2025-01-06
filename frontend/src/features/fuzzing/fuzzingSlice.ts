import {createSlice, PayloadAction} from "@reduxjs/toolkit";
import {FuzzingPreset, OutFuzzingPreset} from "./types.ts";
import {AppDispatch, RootState} from "../../app/store.ts";
import {getFuzzingPresetsUrl} from "./backapi.ts";
import axios from "axios";

interface FuzzingSlice {
    presets: FuzzingPreset[] | null,
    addModalVisible: boolean,
    adding: boolean
}

const initialState: FuzzingSlice = {
    presets: null,
    addModalVisible: false,
    adding: false
};

const fuzzingSlice = createSlice({
    name: 'fuzzing',
    initialState: initialState,
    reducers: {
        setPresets: (state, action: PayloadAction<FuzzingPreset[]>) => {
            state.presets = action.payload;
        },
        setAddModalVisible: (state, action: PayloadAction<boolean>) => {
            state.addModalVisible = action.payload;
        },
        setAdding: (state, action: PayloadAction<boolean>) => {
            state.adding = action.payload;
        }
    }
});

const {
    setPresets,
    setAddModalVisible,
    setAdding
} = fuzzingSlice.actions;

const
    loadPresets = () => (dispatch: AppDispatch) => {
        const loader = async () => {
            try {
                const { data } = await axios.get(getFuzzingPresetsUrl());
                dispatch(setPresets(data))
            } catch {
                return;
            }
        };

        loader();
    },
    showAddModal = () => (dispatch: AppDispatch) => {
        dispatch(setAddModalVisible(true));
    },
    hideAddModal = () => (dispatch: AppDispatch) => {
        dispatch(setAddModalVisible(false));
    },
    addPreset = (preset: OutFuzzingPreset) => (dispatch: AppDispatch) => {
        const adder = async () => {
            dispatch(setAdding(true));

            await axios.post(getFuzzingPresetsUrl(), preset);

            dispatch(hideAddModal());
            dispatch(loadPresets());
            dispatch(setAdding(false));
        };

        adder();
    };

const
    selectPresets = (state: RootState) => state.fuzzing.presets,
    selectAddModalVisible = (state: RootState) => state.fuzzing.addModalVisible,
    selectAdding = (state: RootState) => state.fuzzing.adding;

export default fuzzingSlice.reducer;

export {
    selectPresets,
    loadPresets,
    selectAddModalVisible,
    showAddModal,
    hideAddModal,
    addPreset,
    selectAdding
};