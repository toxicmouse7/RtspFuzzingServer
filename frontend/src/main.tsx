import {createRoot} from 'react-dom/client'
import './index.css'
import AppLayout from './AppLayout.tsx'
import {BrowserRouter, Navigate, Route, Routes} from "react-router";
import Sessions from "./features/sessions/Sessions.tsx";
import {Provider} from "react-redux";
import {store} from "./app/store.ts";
import Inspection from "./features/sessions/Inspection/Inspection.tsx";
import FuzzingPresets from "./features/fuzzing/FuzzingPresets/FuzzingPresets.tsx";

createRoot(document.getElementById('root')!).render(
    <Provider store={store}>
        <BrowserRouter>
            <Routes>
                <Route element={<AppLayout/>}>
                    <Route path={"/"} element={<Navigate to={"sessions"} replace/>}/>
                    <Route path={"sessions"}>
                        <Route index element={<Sessions/>}/>
                        <Route path={":sessionId"} element={<Inspection/>}/>
                    </Route>
                    <Route path={"presets"}>
                        <Route index element={<FuzzingPresets/>}/>
                    </Route>
                </Route>
            </Routes>
        </BrowserRouter>
    </Provider>,
)
