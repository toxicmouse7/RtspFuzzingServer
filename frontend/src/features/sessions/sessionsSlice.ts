import {createSlice, PayloadAction} from "@reduxjs/toolkit";
import type { Session } from "./types.ts";
import {AppDispatch, RootState} from "../../app/store.ts";
import {getSessionsUrl, getSessionUrl} from "./backapi.ts";
import axios from 'axios';

export interface SessionsSlice {
    sessions: Session[] | null,
    fuzzingModalVisible: boolean,
    currentSessionId: string | null
}

const initialState: SessionsSlice = {
    sessions: null,
    fuzzingModalVisible: false,
    currentSessionId: null
};

export const sessionsSlice = createSlice({
   name: 'sessions',
   initialState,
   reducers: {
       setSessions: (state, action: PayloadAction<Session[]>) => {
           state.sessions = action.payload
       },
       setFuzzingModalVisible: (state, action: PayloadAction<boolean>) => {
           state.fuzzingModalVisible = action.payload;
       },
       setCurrentSessionId: (state, action: PayloadAction<string | null>) => {
           state.currentSessionId = action.payload;
       }
   }
});

const {
    setSessions,
    setFuzzingModalVisible,
    setCurrentSessionId
} = sessionsSlice.actions;

const
    loadSessions = () => (dispatch: AppDispatch) => {
        const loader = async () => {
            try {
                const { data } = await axios.get<Session[]>(getSessionsUrl());
                dispatch(setSessions(data))
            } catch {
                return;
            }
        };

        loader();
    },
    removeSession = (id: string) => (dispatch: AppDispatch, getState: () => RootState) => {
        const sessions = getState().sessions.sessions;

        if (!sessions) {
            return;
        }

        const remover = async () => {
            try {
                await axios.delete(getSessionUrl(id));
                dispatch(setSessions(sessions.filter(i => i.id !== id)))
            } catch {
                return;
            }
        };

        remover();
    },
    addSession = (session: Session) => (dispatch: AppDispatch, getState: () => RootState) => {
        const sessions = getState().sessions.sessions ?? [];
        dispatch(setSessions([...sessions, session]))
    },
    showFuzzingModal = (id: string) => (dispatch: AppDispatch) => {
        dispatch(setCurrentSessionId(id));
        dispatch(setFuzzingModalVisible(true))
    },
    hideFuzzingModal = () => (dispatch: AppDispatch) => {
        dispatch(setFuzzingModalVisible(false))
        dispatch(setCurrentSessionId(null));
    };

const
    selectSessions = (state: RootState) => state.sessions.sessions,
    selectFuzzingModalVisible = (state: RootState) => state.sessions.fuzzingModalVisible,
    selectCurrentSessionId = (state: RootState) => state.sessions.currentSessionId;

export default sessionsSlice.reducer;

export {
    selectSessions,
    loadSessions,
    removeSession,
    addSession,
    setSessions,
    selectFuzzingModalVisible,
    showFuzzingModal,
    hideFuzzingModal,
    selectCurrentSessionId
}