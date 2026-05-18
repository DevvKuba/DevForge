import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MemberBlogsComponent } from './member-blogs.component';

describe('MemberBlogsComponent', () => {
  let component: MemberBlogsComponent;
  let fixture: ComponentFixture<MemberBlogsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MemberBlogsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MemberBlogsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
