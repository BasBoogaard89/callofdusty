/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { BaseHttpRequest } from './core/BaseHttpRequest';
import type { OpenAPIConfig } from './core/OpenAPI';
import { AxiosHttpRequest } from './core/AxiosHttpRequest';
import { AuthService } from './services/AuthService';
import { ChoreService } from './services/ChoreService';
import { ChoreCategoryService } from './services/ChoreCategoryService';
import { HistoryService } from './services/HistoryService';
import { RoomService } from './services/RoomService';
import { RoomCategoryService } from './services/RoomCategoryService';
import { TextFragmentService } from './services/TextFragmentService';
import { TextTemplateService } from './services/TextTemplateService';
import { ThemeService } from './services/ThemeService';
type HttpRequestConstructor = new (config: OpenAPIConfig) => BaseHttpRequest;
export class OpenAPIClient {
    public readonly auth: AuthService;
    public readonly chore: ChoreService;
    public readonly choreCategory: ChoreCategoryService;
    public readonly history: HistoryService;
    public readonly room: RoomService;
    public readonly roomCategory: RoomCategoryService;
    public readonly textFragment: TextFragmentService;
    public readonly textTemplate: TextTemplateService;
    public readonly theme: ThemeService;
    public readonly request: BaseHttpRequest;
    constructor(config?: Partial<OpenAPIConfig>, HttpRequest: HttpRequestConstructor = AxiosHttpRequest) {
        this.request = new HttpRequest({
            BASE: config?.BASE ?? 'http://localhost:5177',
            VERSION: config?.VERSION ?? '1.0.0',
            WITH_CREDENTIALS: config?.WITH_CREDENTIALS ?? false,
            CREDENTIALS: config?.CREDENTIALS ?? 'include',
            TOKEN: config?.TOKEN,
            USERNAME: config?.USERNAME,
            PASSWORD: config?.PASSWORD,
            HEADERS: config?.HEADERS,
            ENCODE_PATH: config?.ENCODE_PATH,
        });
        this.auth = new AuthService(this.request);
        this.chore = new ChoreService(this.request);
        this.choreCategory = new ChoreCategoryService(this.request);
        this.history = new HistoryService(this.request);
        this.room = new RoomService(this.request);
        this.roomCategory = new RoomCategoryService(this.request);
        this.textFragment = new TextFragmentService(this.request);
        this.textTemplate = new TextTemplateService(this.request);
        this.theme = new ThemeService(this.request);
    }
}

