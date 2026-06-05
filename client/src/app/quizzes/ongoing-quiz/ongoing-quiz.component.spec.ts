import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OngoingQuizComponent } from './ongoing-quiz.component';

describe('OngoingQuizComponent', () => {
  let component: OngoingQuizComponent;
  let fixture: ComponentFixture<OngoingQuizComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OngoingQuizComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OngoingQuizComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
