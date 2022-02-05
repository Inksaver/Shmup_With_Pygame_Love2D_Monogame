import pygame, random
import shared, circle

class Pow():
	def __init__(self, images, center):
		self.type = random.choice(['shield', 'gun'])
		self.images = images
		self.image = self.images[self.type]
		self.image.set_colorkey(shared.BLACK)	
		self.rect = self.image.get_rect()
		self.rect.center = center
		self.speedy = 500
		self.active = True
		self.circle = circle.Circle(self.rect.centerx, self.rect.centery, self.rect.width / 2)
		self.colour = shared.RED
	
	def get_rect(self):
		return self.rect

	def get_circle(self):
		self.circle.x = self.rect.centerx
		self.circle.y = self.rect.y	+ self.circle.radius	
		return self.circle		

	def update(self, dt):
		self.rect.y += self.speedy * dt
		# delete if moves off bottom of screen
		if self.rect.top > shared.HEIGHT:
			self.active = False
			
		return self.active
			
	def draw(self) -> None:
		shared.screen.blit(self.image, self.rect)
		if shared.debug:
			pygame.draw.circle(shared.screen, self.colour, self.circle.center, self.circle.radius, 1)	