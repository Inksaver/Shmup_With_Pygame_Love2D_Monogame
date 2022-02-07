import pygame, random, math
import shared, circle

class Mob():
	def __init__(self, image:object) -> None:
		self.image_original = image 									# keep a copy of the original image					
		self.image_original.set_colorkey(shared.BLACK)					# set transparency
		self.image = self.image_original.copy()							# self.image is now a copy of the original
		self.rotation = 0
		self.rotation_speed = 0
		self.time_passed = 0
		self._rect = self.image.get_rect()								# get rectangle from image size
		radius = int(min(self._rect.width, self._rect.height) * 0.4) 	# local variable to set circle radius	
		self._circle = circle.Circle(self._rect.centerx, self._rect.centery, radius) # using circle collision
		
		self.reset()

	def reset(self) -> None:
		# set x coord allowing for the width of the sprite itself
		self._rect.x = random.randrange(0, shared.WIDTH - self._rect.width)
		# start off the top of the screen by random amount
		self._rect.y = random.randrange(-150, -100)
		# set speed to a random amount, so some will move faster than others
		self.speedy = random.randrange(50, 400)
		self.speedx = random.randrange(-50, 50)
		if shared.debug:	# reduce speeds in debug mode
			self.speedy = random.randrange(30, 100)
			self.speedx = random.randrange(-20, 20)
		self.rotation_speed = random.randrange(-10, 10)
		self._circle.center = self._rect.center

	@property
	def rect(self):
		return self._rect

	@property
	def circle(self):
		return self._circle	

	def rotate(self, dt):
		''' rotates a copy of the original image '''
		self.time_passed += dt
		if self.time_passed >= 0.05:												# has 50ms passed?
			self.time_passed = 0													# reset timer to 0
			self.rotation = (self.rotation + self.rotation_speed) % 360 			# rotation autoresets to 0
			new_image = pygame.transform.rotate(self.image_original, self.rotation) # new rotated image
			old_centre = self._rect.center											# store x,y
			self.image = new_image													# replace self.image
			self._rect = self.image.get_rect()										# get rectangle of new image
			self._rect.center = old_centre											# set centre to original x,y
			self._circle.center = self._rect.center									# set circle centre
   
	def get_rotated_vertices(self):
		''' gets the vertices of a rotated rectangle for pygame.draw.polygon '''
		r = math.sqrt(self._rect.width * self._rect.width / 4 + self._rect.height * self._rect.height / 4)
		
		thetas = [math.degrees(math.atan((self._rect.height / 2) / (self._rect.width / 2)))]
		thetas.extend([-thetas[0] - self.rotation,
					   thetas[0] - 180 - self.rotation,
					   180 - thetas[0] - self.rotation])

		thetas[0] -= self.rotation
		vertices = []
		for theta in thetas:
			vertices.append(pygame.math.Vector2(math.cos(math.radians(theta)) * r + self._rect.centerx,
												math.sin(math.radians(theta)) * r + self._rect.centery))

		return vertices		

	def update(self, dt:float) -> None:
		self.rotate(dt)								# rotate the image
		self._rect.y += round(self.speedy * dt)		# change y coord
		self._rect.x += round(self.speedx * dt)		# change x coord
		self._circle.center = self._rect.center		# match circle centre with rectangle
		if	self._rect.top > shared.HEIGHT + 10 or self._rect.left < -25 or self._rect.right > shared.WIDTH + 20:
			self.reset()

	def draw(self) -> None:
		shared.screen.blit(self.image, self._rect) 	# draw the image
		if shared.debug:							# debug mode draw circle and rectangles around image
			pygame.draw.circle(shared.screen, shared.RED, self._circle.center, self._circle.radius, 1)
			pygame.draw.rect(shared.screen, shared.BLUE, self._rect, 1)
			pygame.draw.polygon(shared.screen, shared.GREEN, self.get_rotated_vertices(), 1)