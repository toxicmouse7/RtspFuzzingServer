import {configureStore} from "@reduxjs/toolkit";
import sessionsReducer from "../features/sessions/sessionsSlice.ts";
import fuzzingReducer from "../features/fuzzing/fuzzingSlice.ts";

export const store = configureStore({
    reducer: {
        sessions: sessionsReducer,
        fuzzing: fuzzingReducer
    }
});

export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch;