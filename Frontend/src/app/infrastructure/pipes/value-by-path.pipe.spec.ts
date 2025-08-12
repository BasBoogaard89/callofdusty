import { ValueByPathPipe } from './value-by-path.pipe';

describe('ValueByPathPipe', () => {
    it('create an instance', () => {
        const pipe = new ValueByPathPipe();
        expect(pipe).toBeTruthy();
    });
});
